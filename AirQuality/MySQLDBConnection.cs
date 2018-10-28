
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System;

namespace AirQuality
{
    class MySqlDBConnection
    {
        //Atributes
        public MySqlConnection connection;
        public MySqlDataAdapter adapter;
        public string server;
        public string database;
        public string uid;
        public string password;
        private SendEmail se;

        //Constructor
        public MySqlDBConnection()
        {
            se = new SendEmail();
            server = "localhost";
            database = "airqualitydb";
            uid = "root";
            password = "jasimaoIPG2018";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }
        //open connection to database
         public bool OpenConnection()
         {
             try
             {
                 connection.Open();
                 return true;
             }
             catch (MySqlException ex)
             {
                 switch (ex.Number)
                 {
                     case 0:

                        se.Body = "Cannot connect to server.  Contact administrator";
                        se.SendAlarm();
                         break;
                     case 1045:

                        se.Body = "Cannot connect to server.  Contact administrator";
                        se.SendAlarm();
                        break;
                 }
                 return false;
             }
         }
         //Close connection
         public bool CloseConnection()
         {
             try
             {
                 connection.Close();
                 return true;
             }
             catch (MySqlException ex)
             {
                se.Body = "" + ex.Message;
                se.SendAlarm();
                MessageBox.Show(ex.Message);
                return false;
             }
         }
         //Insert statement
         public void Insert( string s)
         {
             string query = "INSERT INTO sensorvalue (co2,voc,rh,t,pm25,hi,dp,date) VALUES" + s;

             //open connection
             if (OpenConnection() == true)
             {
                 //create command and assign the query and connection from the constructor
                 MySqlCommand cmd = new MySqlCommand(query, connection);

                 //Execute command
                 cmd.ExecuteNonQuery();

                 //close connection
                CloseConnection();
             }
         }
        //Select all values from the database statement
        public DataTable SelectAll()
        {
            string query = "SELECT * from sensorvalue";
            //open connection
            try
            {
                OpenConnection();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                cmd.ExecuteNonQuery();
                adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
            catch (MySqlException ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                //close connection
                CloseConnection();
            }
        }
        public SensorValue SelectLastEntry()
        {
            string query = "SELECT * FROM sensorvalue ORDER BY ID DESC LIMIT 1";
            //open connection
            try
            {
                OpenConnection();
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Execute command
                 cmd.ExecuteNonQuery();
                adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                //SensorValues auxiliar
                SensorValue aux = new SensorValue();
                aux.c = Convert.ToInt32(dataTable.Rows[0][1]);
                aux.v = Convert.ToInt32(dataTable.Rows[0][2]);
                aux.r = Convert.ToInt32(dataTable.Rows[0][3]);
                aux.temp = Convert.ToInt32(dataTable.Rows[0][4]);
                aux.pm = Convert.ToInt32(dataTable.Rows[0][5]);
                aux.h = Convert.ToInt32(dataTable.Rows[0][6]);
                aux.d = Convert.ToInt32(dataTable.Rows[0][7]);
                //return dt.ToString();
                return aux;
            }
            catch (MySqlException ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                //close connection
                CloseConnection();
            }
        }
    }
}
