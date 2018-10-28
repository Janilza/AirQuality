using System;

namespace AirQuality
{
    class SensorValue
    {
        //variables definition
        private int co2;
        private int voc;
        private float rh;
        private float t;
        private int pm25;
        private float hi;
        private float dp;
        DateTime date;
        
        public SensorValue()
        {
            co2 = 400;
            voc = 0;
            rh = 23 ;
            t = 27;
            pm25 = 2;
            hi = 29;
            dp = 10;
            date = DateTime.Now;
        }
        public SensorValue(int c, int v, float r, float temp, int p, float h, float d)
        {
            co2 = c;
            voc = v;
            rh = r;
            t = temp;
            pm25 = p;
            hi = h;
            dp = d;
            date = DateTime.Now;
        }
        //gets and sets
        public int c
        {
            get { return co2; }
            set { co2 = value; }
        }
        public int v
        {
            get { return voc; }
            set { voc = value; }
        }
        public float r
        {
            get { return rh; }
            set { rh = value; }
        }
        public float temp
        {
            get { return t; }
            set { t = value; }
        }
        public int pm
        {
            get { return pm25; }
            set { pm25 = value; }
        }
        public float h
        {
            get { return hi; }
            set { hi = value; }
        }
        public float d
        {
            get { return dp; }
            set { dp = value; }
        }
        public DateTime dateTime {
            get { return date; }
            set { date = value; }
        }

        // this functions validate the data of the sensors
        public string cStatus() {
            
            string result="";
           
            if (co2 >= 0 && co2 < 1250)
            {
                result = "good";
            }
            else if (co2 >= 1250 && co2 < 5000)
            {
                result = "moderate";
            }
            else if(co2 >= 5000){
                result = "bad";
            }
            return result;
        }

        public string vStatus()
        {
            string result = "";
            if (voc == 0 || voc == 1)
            {
                result = "good";
            }
            else if (voc == 2)
            {
                result = "moderate";
            }
            else if (voc == 3)
            {
                result = "bad";
            }
            return result;
        }

        public string rStatus()
        {
            string result = "";
            if (rh >= 0 && rh < 55) // >=40
            {
                result = "good";
            }
            else if (rh >= 55 && rh < 60)
            {
                result = "moderate";
            }
            else if (rh >= 60)
            {
                result = "bad";
            }
            return result;
        }

        public string tStatus()
        {
            string result = "";
            if (t >= 20 && t < 26)
            {
                result = "good";
            }
            else if (t >= 26 && t < 30)
            {
                result = "moderate";
            }
            else if (t >= 30)
            {
                result = "bad";
            }
            return result;
        }

        public string pStatus()
        {
            string result = "";
            if (pm25 >= 0 && pm25 < 50)
            {
                result = "good";
            }
            else if (pm25 >= 50 && pm25 < 100)
            {
                result = "moderate";
            }
            else if (pm25 >= 100)
            {
                result = "bad";
            }
            return result;
        }
        public string hStatus()
        {
            return tStatus();
        }
        public string dStatus()
        {
            return rStatus();
        }

        public override string ToString()
        {
            return "\nCO2: " + c + "\nVOCs: " + v + "\nRH: " + r + "\nT: " + temp + "\nPM2.5: " + pm + "\nHI: " + h + "\nDP: " + d + "\nDate: "+ dateTime;
        }

        //send to db
        public string ToStringDB()
        {
            return "('" + c + "','" + v + "','" + r + "','" + temp + "','" + pm + "', '" + h + "','" + d + "','" + dateTime + "')";
        }
    }
}
