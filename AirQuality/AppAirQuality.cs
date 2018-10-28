
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace AirQuality
{
    class AppAirQuality
    {
        //conf screen
        private float spaceInicialX;
        private float spaceInicialY;
        private float spaceInicialY1;
        private float firstQueueWidth;
        private float secondQueueWidth;
        private float queueHeight;
        private float headerWidth;
        private float legendHeight;
        private float screenHeight;
        private float screenWidth;
        private string result;
        //labels
        private string tLabel;
        private string cLabel;
        private string dLabel;
        private string pLabel;
        private string humLabel;
        private string vLabel;
        private string hLabel;
        //sensor value labels
        private string tValue;
        private string cValue;
        private string dValue;
        private string pValue;
        private string humValue;
        private string vValue;
        private string hValue;
        //sensor units
        private string tUnit;
        private string cUnit;
        private string dUnit;
        private string pUnit;
        private string humUnit;
        private string vUnit;
        private string hUnit;
        //foot labels
        private string goodLabel;
        private string moderateLabel;
        private string badLabel;

        private string titleLabel;

        private string big;
        private string medium;
        private string small;

        private SendEmail se;

        private Button config;
        Config cf;

        SolidBrush fontBrush;
        StringFormat sf;
        MySqlDBConnection mySqlDB;

        //Last Value 
        SensorValue aux;

        public AppAirQuality(int width, int height, MySqlDBConnection mySql)
        {
           
            tLabel = "Temperatura";
            cLabel= "CO₂";
            dLabel= "Ponto de Orvalho";
            pLabel = "PM2.5";
            humLabel = "Humidade";
            vLabel = "VOCs";
            hLabel = "Sensação Térmica";

            tUnit = "°C";
            cUnit = "ppm";
            dUnit = "°C";
            pUnit ="ug/m3";
            humUnit = "%";
            vUnit = "";
            hUnit = "°C";

            goodLabel = "Bom";
            moderateLabel = "Moderado";
            badLabel = "Mau";
            titleLabel = "Monitorização da Qualidade do ar";

            big = "Big";
            medium = "Medium";
            small = "Small";

            se = new SendEmail();

            fontBrush = new SolidBrush(Color.FromArgb(8, 128, 186));
            sf = new StringFormat();

            config = new Button();
            cf = new Config();

            spaceInicialY = 0.2F * height;
            spaceInicialY1 = 0.25F * height;
            spaceInicialX = 0.05F * width;
            queueHeight = 0.35F * height;
            firstQueueWidth = 0.33F * width;
            secondQueueWidth = 0.25F * width;
            headerWidth = width;
            legendHeight = 0.10F * height;
            screenHeight = height;
            screenWidth = width;
            result = Resolution(width, height);

            mySqlDB = mySql;

            //Last value
            aux = new SensorValue();
        }
        public float SpaceInicialY
        {
            get { return spaceInicialY; }  
        }
        public float SpaceInicialX
        {
            get { return spaceInicialX; }
        }
        public float FirstQueueWidth
        {
            get { return firstQueueWidth; }
        }
        public float SecondQueueWidth
        {
            get { return secondQueueWidth; }
        }
        public float QueueHeight
        {
            get { return queueHeight; }
        }
        public float HeaderWidth
        {
            get { return headerWidth; }
        }
        public float LegendHeight
        {
            get { return legendHeight; }
        }
        public float ScreenHeight
        {
            get { return screenHeight; }
        }
        public float ScreenWidth
        {
            get { return screenWidth; }
        }
        public string Result
        {
            get { return result; }
        }
        private string Resolution(int w, int h)
        {
            string result = "";
            if ((w > 1024) && (h >= 1080))
            {
                result = big;
            }
            else if ((w > 800 || w <= 1024) && (h >= 600 || h < 1080))
            {
                result = medium;
            }
            else if ((w >= 0 || w <= 800) && (h >= 0 || h <= 600))
            {
                result = small;
            }
            return result;
        }
        public void updateValues(Graphics g, Stack<SensorValue> allValues)
        {
            Font valuesFont = new Font("Microsoft Sans Serif", 5, FontStyle.Bold);

            if (Result.Equals(small) || Result.Equals(medium))
            {
                valuesFont = new Font("Microsoft Sans Serif", 40 * 0.5F, FontStyle.Bold);
            }else if (Result.Equals(big))
            {
                valuesFont = new Font("Microsoft Sans Serif", 40, FontStyle.Bold);
            }
            //Sensor value labels
            tValue = allValues.Peek().temp + tUnit;
            humValue = allValues.Peek().r + humUnit;
            hValue = allValues.Peek().h + hUnit;
            dValue = allValues.Peek().d + dUnit;
            cValue = allValues.Peek().c + cUnit;
            pValue = allValues.Peek().pm + pUnit;
            vValue = "Nível " + allValues.Peek().v + vUnit;
            
            //CO2 value
            SizeF size = g.MeasureString(cValue, valuesFont);
            g.DrawString(cValue, valuesFont, fontBrush, SpaceInicialX + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY1 + QueueHeight) + (QueueHeight * 0.9F) * 0.40F, sf);
            
            //PM2.5 value
            size = g.MeasureString(pValue, valuesFont);
            g.DrawString(pValue, valuesFont, fontBrush, FirstQueueWidth + SpaceInicialX + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY1 + QueueHeight) + (QueueHeight * 0.9F) * 0.40F);
        
            //VOC value
            size = g.MeasureString(vValue, valuesFont);
            g.DrawString(vValue, valuesFont, fontBrush, 2 * FirstQueueWidth + SpaceInicialX + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY1 + QueueHeight) + (QueueHeight * 0.9F) * 0.40F);

            //Temperature value
            size = g.MeasureString(tValue, valuesFont);
            g.DrawString(tValue, valuesFont, fontBrush, SpaceInicialX / 2 + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (SpaceInicialY) + (QueueHeight * 0.9F) * 0.40F);

            //Humidity value
            size = g.MeasureString(humValue, valuesFont);
            g.DrawString(humValue, valuesFont, fontBrush, SecondQueueWidth + SpaceInicialX / 2 + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (SpaceInicialY) + (QueueHeight * 0.9F) * 0.40F);

            // Heat Index value
            size = g.MeasureString(hValue, valuesFont);
            g.DrawString(hValue, valuesFont, fontBrush, 2 * SecondQueueWidth +SpaceInicialX / 2 + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (SpaceInicialY) + (QueueHeight * 0.9F) * 0.40F);

            //DewPoint value
            size = g.MeasureString(dValue, valuesFont);
            g.DrawString(dValue, valuesFont, fontBrush, 3 * SecondQueueWidth + SpaceInicialX / 2 + (QueueHeight * 0.9F) * 0.5F - (size.Width / 2), (SpaceInicialY) + (QueueHeight * 0.9F) * 0.40F);
        }
        public void config_click(object sender, EventArgs e) {
            cf.Show();
        }
        public void paintForm(object sender, PaintEventArgs e, Stack<SensorValue> allValues)
        {
            //Bitmap bm = new Bitmap((int)ScreenWidth, (int)ScreenHeight, PixelFormat.Format24bppRgb);

            //Graphics g = Graphics.FromImage(bm);
            Graphics g = e.Graphics;
            g.Clear(Color.White); // Set Bitmap background to white

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//make the graphics better 
            g.TextRenderingHint = TextRenderingHint.AntiAlias;//text format

            Font titleFont = new Font("Microsoft Sans Serif", 5, FontStyle.Bold);
            Font labelsFont = new Font("Microsoft Sans Serif", 5, FontStyle.Bold);
            Font footerFont = new Font("Microsoft Sans Serif", 5, FontStyle.Bold);
            
            RectangleF rCO2 = new RectangleF(spaceInicialX, spaceInicialY1 + queueHeight, queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rPM = new RectangleF(spaceInicialX + firstQueueWidth, spaceInicialY1 + queueHeight, queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rVOC = new RectangleF(spaceInicialX + 2 * firstQueueWidth, spaceInicialY1 + queueHeight, queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rT = new RectangleF(spaceInicialX / 2, spaceInicialY , queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rRH = new RectangleF(spaceInicialX / 2 + secondQueueWidth, spaceInicialY, queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rHI = new RectangleF(spaceInicialX / 2 + 2 * secondQueueWidth, spaceInicialY, queueHeight * 0.90F, queueHeight * 0.90F);
            RectangleF rDP = new RectangleF(spaceInicialX / 2 + 3 * secondQueueWidth, spaceInicialY, queueHeight * 0.90F, queueHeight * 0.90F); // spaceInicialY + queueHeight

            //legend
            RectangleF rGood = new RectangleF(secondQueueWidth - legendHeight * 0.2F, screenHeight * 0.9F + legendHeight * 0.50F, legendHeight * 0.2F, legendHeight * 0.2F);
            RectangleF rModerate = new RectangleF(2 * secondQueueWidth - legendHeight * 0.2F, screenHeight * 0.9F + legendHeight * 0.50F, legendHeight * 0.2F, legendHeight * 0.2F);
            RectangleF rBad = new RectangleF(3 * secondQueueWidth - legendHeight * 0.2F, screenHeight * 0.9F + legendHeight * 0.50F, legendHeight * 0.2F, legendHeight * 0.2F);

            //colors
            Color red = Color.FromArgb(196, 2, 8);
            Color green = Color.FromArgb(0, 77, 13);
            Color yellow = Color.FromArgb(255, 228, 56);

            //Pens, brushes
            SolidBrush redBrush = new SolidBrush(red);
            SolidBrush greenBrush = new SolidBrush(green);
            SolidBrush yellowBrush = new SolidBrush(yellow);

            Pen greenPen = new Pen(green, 30);
            Pen yellowPen = new Pen(yellow, 30);
            Pen redPen = new Pen(red, 30);

            Pen greenPenLegend = new Pen(green, 5);
            Pen yellowPenLegend = new Pen(yellow, 5);
            Pen redPenLegend = new Pen(red, 5);

            string mailBody = "";

            //legend simbols
            g.DrawEllipse(greenPenLegend, rGood);
            g.DrawEllipse(yellowPenLegend, rModerate);
            g.DrawEllipse(redPenLegend, rBad);
            
            ////temperature
            if (allValues.Peek().tStatus() == "good")
            {
                g.DrawEllipse(greenPen, rT);
            }
            else if (allValues.Peek().tStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rT);
            }
            else if (allValues.Peek().tStatus() == "bad")
            {
                g.DrawEllipse(redPen, rT);
                if (aux.tStatus() == "good" || aux.tStatus() == "moderate")
                {
                    mailBody += "\nTemperatura: " + allValues.Peek().temp + tUnit;
                }
            }
            ////co2
            if (allValues.Peek().cStatus() == "good")
            {
                g.DrawEllipse(greenPen, rCO2);
            }
            else if (allValues.Peek().cStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rCO2);
            }
            else if (allValues.Peek().cStatus() == "bad")
            {
                g.DrawEllipse(redPen, rCO2);
                if (aux.cStatus() == "good" || aux.cStatus() == "moderate")
                {
                    mailBody += "\nCO2: " + allValues.Peek().c + cUnit;
                } 
            }
            ////Dew Point
            if (allValues.Peek().dStatus() == "good")
            {
                g.DrawEllipse(greenPen, rDP);
            }
            else if (allValues.Peek().dStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rDP);
            }
            else if (allValues.Peek().dStatus() == "bad")
            {
                g.DrawEllipse(redPen, rDP);
                if (aux.dStatus() == "good" || aux.dStatus() == "moderate")
                {
                    mailBody += "\nPonto de orvalho: " + allValues.Peek().d + dUnit;
                }
            }
            ////pm25
            if (allValues.Peek().pStatus() == "good")
            {
                g.DrawEllipse(greenPen, rPM);
            }
            else if (allValues.Peek().pStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rPM);
            }
            else if (allValues.Peek().pStatus() == "bad")
            {
                g.DrawEllipse(redPen, rPM);
                if (aux.pStatus() == "good" || aux.pStatus() == "moderate")
                {
                    mailBody += "\nPM2.5: " + allValues.Peek().pm + pUnit;
                }
            }
            ////Relative Humidity
            if (allValues.Peek().rStatus() == "good")
            {
                g.DrawEllipse(greenPen, rRH);
            }
            else if (allValues.Peek().rStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rRH);
            }
            else if (allValues.Peek().rStatus() == "bad")
            {
                g.DrawEllipse(redPen, rRH);
                if (aux.rStatus() == "good" || aux.rStatus() == "moderate")
                {
                    mailBody += "\nHumidade: " + allValues.Peek().r + humUnit;
                }
            }
            ////VOC
            if (allValues.Peek().vStatus() == "good")
            {
                g.DrawEllipse(greenPen, rVOC);
            }
            else if (allValues.Peek().vStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rVOC);
            }
            else if (allValues.Peek().vStatus() == "bad")
            {
                g.DrawEllipse(redPen, rVOC);
                if (aux.vStatus() == "good" || aux.vStatus() == "moderate")
                {
                    mailBody += "\nVOC: " + allValues.Peek().v + vUnit;
                }
            }
            ////HI
            if (allValues.Peek().hStatus() == "good")
            {
                g.DrawEllipse(greenPen, rHI);
            }
            else if (allValues.Peek().hStatus() == "moderate")
            {
                g.DrawEllipse(yellowPen, rHI);
            }
            else if (allValues.Peek().hStatus() == "bad")
            {
                g.DrawEllipse(redPen, rHI);
                if (aux.hStatus() == "good" || aux.hStatus() == "moderate")
                {
                   mailBody += "\nSensação térmica: " + allValues.Peek().h + hUnit;
                }
            }
            //labels 
            if (Result.Equals(small) || Result.Equals(medium))
            {
                titleFont = new Font("Microsoft Sans Serif", 50 * 0.5F, FontStyle.Bold);
                labelsFont = new Font("Microsoft Sans Serif", 20 * 0.5F, FontStyle.Bold);
                footerFont = new Font("Microsoft Sans Serif", 20 * 0.5F, FontStyle.Bold);

            }else if (Result.Equals(big))
            {
                titleFont = new Font("Microsoft Sans Serif", 50, FontStyle.Bold);
                labelsFont = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
                footerFont = new Font("Microsoft Sans Serif", 20, FontStyle.Bold);
            }
            //title
            SizeF size = g.MeasureString(titleLabel, titleFont);
            g.DrawString(titleLabel, titleFont, fontBrush, (headerWidth - size.Width) / 2, (screenHeight * 0.2F - size.Height) / 2, sf);

            //CO2
            size = g.MeasureString(cLabel, labelsFont); 
            g.DrawString(cLabel, labelsFont, fontBrush, spaceInicialX + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.20F, sf);

            //PM2.5
            size = g.MeasureString(pLabel, labelsFont);
            g.DrawString(pLabel, labelsFont, fontBrush, firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.20F, sf);

            //VOC
            size = g.MeasureString(vLabel, labelsFont);
            g.DrawString(vLabel, labelsFont, fontBrush, 2 * firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.20F, sf);

            //Temperature
            size = g.MeasureString(tLabel, labelsFont);
            g.DrawString(tLabel, labelsFont, fontBrush, spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY ) + (queueHeight * 0.9F) * 0.20F, sf);

            //Humidity
            size = g.MeasureString(humLabel, labelsFont);
            g.DrawString(humLabel, labelsFont, fontBrush, secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY) + (queueHeight * 0.9F) * 0.20F, sf);

            //Heat Index
            size = g.MeasureString(hLabel, labelsFont);
            g.DrawString(hLabel, labelsFont, fontBrush, 2 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY) + (queueHeight * 0.9F) * 0.20F, sf);

            //Dew Point
            size = g.MeasureString(dLabel, labelsFont);
            g.DrawString(dLabel, labelsFont, fontBrush, 3 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (size.Width / 2), (spaceInicialY) + (queueHeight * 0.9F) * 0.20F, sf);
           
            //update the sensor values
            updateValues(g, allValues);

            //footer
            size = g.MeasureString(goodLabel, footerFont);
            g.DrawString(goodLabel, footerFont, fontBrush, secondQueueWidth*0.02F + secondQueueWidth, screenHeight * 0.9F + legendHeight * 0.50F + (legendHeight * 0.2F - size.Height) / 2, sf);

            size = g.MeasureString(moderateLabel, footerFont);
            g.DrawString(moderateLabel, footerFont, fontBrush, 2.02F * secondQueueWidth, screenHeight * 0.9F + legendHeight * 0.50F + (legendHeight * 0.2F - size.Height) / 2, sf);

            size = g.MeasureString(badLabel, footerFont);
            g.DrawString(badLabel, footerFont, fontBrush, 3.02F * secondQueueWidth, screenHeight * 0.9F + legendHeight * 0.50F + (legendHeight * 0.2F - size.Height) / 2, sf);

            //logos
            if (Result.Equals(small) || Result.Equals(medium)) {

                Image airQuality = Image.FromFile("logoApp_.png");
                g.DrawImage(airQuality, new PointF((headerWidth * 0.2F - airQuality.Width) / 2, (screenHeight * 0.20F - airQuality.Height) / 2));
                
                Image ulsg = Image.FromFile("ulsLogo_.png");
                g.DrawImage(ulsg, new PointF(headerWidth * 0.8F + (headerWidth * 0.2F - airQuality.Width) / 2, (screenHeight * 0.20F - ulsg.Height) / 2));

                Image temperature = Image.FromFile("co2_.png");
                g.DrawImage(temperature, new PointF(spaceInicialX + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image co2 = Image.FromFile("pm_.png");
                g.DrawImage(co2, new PointF(firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (co2.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image dp = Image.FromFile("voc_.png");
                g.DrawImage(dp, new PointF(2 * firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (dp.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image pm = Image.FromFile("temp_.png");
                g.DrawImage(pm, new PointF(spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));

                Image humidity = Image.FromFile("hm_.png");
                g.DrawImage(humidity, new PointF(secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));

                Image voc = Image.FromFile("hi_.png");
                g.DrawImage(voc, new PointF(2 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));

                Image hi = Image.FromFile("dp_.png");
                g.DrawImage(hi, new PointF(3 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));
            }
            else if(result.Equals(big)){

                Image airQuality = Image.FromFile("logoApp.png");
                g.DrawImage(airQuality, new PointF((headerWidth * 0.2F - airQuality.Width) / 2, (screenHeight * 0.20F - airQuality.Height) / 2));
               
                Image ulsg = Image.FromFile("ulsLogo.png");
                g.DrawImage(ulsg, new PointF(headerWidth * 0.8F + (headerWidth * 0.2F - airQuality.Width) / 2, (screenHeight * 0.20F - ulsg.Height) / 2));

                Image temperature = Image.FromFile("co2.png");
                g.DrawImage(temperature, new PointF(spaceInicialX + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image co2 = Image.FromFile("pm.png");
                g.DrawImage(co2, new PointF(firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (co2.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image dp = Image.FromFile("voc.png");
                g.DrawImage(dp, new PointF(2 * firstQueueWidth + spaceInicialX + (queueHeight * 0.9F) * 0.5F - (dp.Width / 2), spaceInicialY1 + queueHeight + (queueHeight * 0.9F) * 0.60F));

                Image pm = Image.FromFile("temp.png");
                g.DrawImage(pm, new PointF(spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY  + (queueHeight * 0.9F) * 0.60F));

                Image humidity = Image.FromFile("hm.png");
                g.DrawImage(humidity, new PointF(secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));

                Image voc = Image.FromFile("hi.png");
                g.DrawImage(voc, new PointF(2 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));

                Image hi = Image.FromFile("dp.png");
                g.DrawImage(hi, new PointF(3 * secondQueueWidth + spaceInicialX / 2 + (queueHeight * 0.9F) * 0.5F - (temperature.Width / 2), spaceInicialY + (queueHeight * 0.9F) * 0.60F));
            }

            // Send Mail Alarm
            if (mailBody.Length > 0)
            {
                se.Body = "Valores muito altos\n " + mailBody + "\nData e Hora: " + allValues.Peek().dateTime;
                se.SendAlarm();
            }
            mailBody = "";
            //Last Value inserted on database 
            aux = mySqlDB.SelectLastEntry();
            Console.WriteLine("Last values inserted on database: \n" + aux.ToString());
        }
    }
}
