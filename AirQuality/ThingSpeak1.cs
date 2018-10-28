
using System;
using System.Collections.Generic;
using System.Net;


namespace AirQuality
{
    class ThingSpeak1
    {
        private string WRITEKEY;
        private string strUpdateBase;
        private string strUpdateURI;
        private HttpWebRequest ThingsSpeakReq;
        private HttpWebResponse ThingsSpeakResp;
        
        public ThingSpeak1()
        {
            WRITEKEY = "O0051IMWSCN1L8SB";
            strUpdateBase = "http://api.thingspeak.com/update";
            strUpdateURI = strUpdateBase + "?api_key=" + WRITEKEY;
         }
        public void sendToThingspeak(Stack<SensorValue> allValues)
        {
            ////send read data to thingSpeak plataform
            
            try
            {
            strUpdateURI += "&field1=" + allValues.Peek().c;
            strUpdateURI += "&field2=" + allValues.Peek().v;
            strUpdateURI += "&field3=" + allValues.Peek().r;
            strUpdateURI += "&field4=" + allValues.Peek().temp;
            strUpdateURI += "&field5=" + allValues.Peek().pm;
            strUpdateURI += "&field6=" + allValues.Peek().h;
            strUpdateURI += "&field7=" + allValues.Peek().d;

            ThingsSpeakReq = (HttpWebRequest)WebRequest.Create(strUpdateURI);
            ThingsSpeakResp = (HttpWebResponse)ThingsSpeakReq.GetResponse();
            if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
            {
                //goto conti;
                //continue;
            }

                if (!(string.Equals(ThingsSpeakResp.StatusDescription, "OK")))
                {

                    Exception exData = new Exception(ThingsSpeakResp.StatusDescription);
                    throw exData;
                }

            }
            catch (WebException ex)
            {
                //goto conti;
                //continue;
                Console.WriteLine("This program is expected to throw WebException on successful run." +
                                    "\n\nException Message :" + ex.Message);
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    Console.WriteLine("Status Code : {0}", ((HttpWebResponse)ex.Response).StatusCode);
                    Console.WriteLine("Status Description : {0}", ((HttpWebResponse)ex.Response).StatusDescription);
                }


            }
            catch (Exception ex)
            {
                //goto conti;
                Console.WriteLine(ex.Message);
            }


        }


    }
}
