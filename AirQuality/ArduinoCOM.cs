using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace AirQuality
{
    class ArduinoCOM
    {
        private readonly int size;
        private readonly string code;
        private bool firstInfo;
        private String s;
        private String[] indata;
        private SensorValue sv;
        private int[] parsedValues;
        private SerialPort sp;

        public ArduinoCOM()
        {
            size = 7;
            code = "AQ_ULSG";
            firstInfo = false;
            sv = new SensorValue();
            s = "";
            
        }
        public float fToCelsius(float f)
        {
            return (float)Math.Round((double)5F/9F*(f - 32F),2);
        }
       
        public void ReceiveData(Object sender, Stack<SensorValue> allValues, MySqlDBConnection db)
        {
           
            Start: Thread.Sleep(5000); //the same on arduino program
            string[] separatingChars = { " " };
            sp = (SerialPort)sender;

            //send handshake code to visual studio
            if (firstInfo == false)
            {
                sp.Write(code);
                firstInfo = true;
            }

            //read serial data from arduino program
            s = sp.ReadExisting();
            indata = s.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries); 
            parsedValues = new int[size];
           
            //verify the length of the packet sent by arduino
            if (indata.Length != size)
            {
                Array.Clear(indata, 0, indata.Length);
                goto Start;
            }
            //parse from string[] to int[]
            for (int i = 0; i < indata.Length; i++)
            {
                int.TryParse(indata[i], out parsedValues[i]);
            }

            Array.Clear(indata, 0, indata.Length);

            sv.c = parsedValues[0];
            sv.v = parsedValues[1];
            sv.r = Convert.ToSingle(parsedValues[2]) / 100;
            sv.temp = Convert.ToSingle(parsedValues[3]) / 100;
            sv.pm = parsedValues[4];
            sv.h = Convert.ToSingle(parsedValues[5]) / 100;
            sv.d = fToCelsius(Convert.ToSingle(parsedValues[6]) / 100);
            sv.dateTime = DateTime.Now;

            Array.Clear(parsedValues, 0, parsedValues.Length);
            allValues.Push(sv); //save last read data on sensorvalues stack
            sv = new SensorValue();

            Console.WriteLine("\n Sensor Values: ");
            Console.Write(allValues.Peek().ToString()); //print last read data
            Console.WriteLine();

        }

    }
    
}
