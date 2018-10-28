
using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;

namespace AirQuality
{
    public partial class FrmPrincipal : Form
    {
        private SensorValue init = new SensorValue();
        private Stack<SensorValue> allValues = new Stack<SensorValue>(); //save all sensor data LIFO
        private MySqlDBConnection db = new MySqlDBConnection();
        private ArduinoCOM ac;
        private ThingSpeak ts;
        private AppAirQuality aq;
        private System.Timers.Timer aTimer;
  
        public FrmPrincipal()
        {
            InitializeComponent();
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
            ClientSize = new Size(this.Width, this.Height);
            
            ac = new ArduinoCOM();
            ts = new ThingSpeak();  
            aq = new AppAirQuality(this.Width, this.Height, db);
            allValues.Push(init);
            //timer1.Start();  antigo timer para o thingSpeak 
            
            //Timer para o envio dos dados para o thingspeak
            aTimer = new System.Timers.Timer(10000); 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void FrmPrincipal_Load(object sender, EventArgs e)
        {
            
            serial.Open();
            serial.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived);
            this.Paint += new PaintEventHandler(this.frmPrincipal_Paint);
           
        }
        //Receive the data from arduino and save on MySQL database
        public void serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ac.ReceiveData(sender, allValues, db);
            db.Insert(allValues.Peek().ToStringDB());
            this.Invalidate();
        }
        public void frmPrincipal_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;  //make the graphics better 
            aq.paintForm(sender, e, allValues);
            g.Dispose();
        }
        //event for send the values to thingSpeak
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Novo envio de dados para o Thingspeak às {0:HH:mm:ss.fff}",e.SignalTime);
            ts.sendToThingspeak(allValues);
        }
        //Test avoid flickering
        public static void enableDoubleBuff(System.Windows.Forms.Control cont)
        {
            System.Reflection.PropertyInfo DemoProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            DemoProp.SetValue(cont, true, null);
        }

    }
}