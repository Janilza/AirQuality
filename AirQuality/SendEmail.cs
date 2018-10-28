using System;
using System.Windows.Forms;
using System.Net.Mail;

/**This class use SMTP protocol to send alert email */

namespace AirQuality
{
    class SendEmail
    {
        private string serverPath;
        private string mailFrom;
        private string password;
        private string mailTo;
        private string subject;
        private string body;
        private int port;

        private MailMessage mail;
        private SmtpClient server;

        public SendEmail()
        {
            serverPath = "smtp.office365.com";
            mailFrom = "airqualityulsg@hotmail.com";
            password = "jasimaoIPG2018";
            mailTo = "jana-simao@hotmail.com";
            subject = "AirQuality alert email";
            body = "AirQuality alert email";
            port = 587;
            mail = new MailMessage();
            server = new SmtpClient(serverPath);
        }
        public SendEmail(string b)
        {
            serverPath = "smtp.office365.com";
            mailFrom = "airqualityulsg@hotmail.com";
            password = "jasimaoIPG2018";
            mailTo = "jana-simao@hotmail.com";
            subject = "AirQuality alert email";
            body = b;
            port = 587;
            mail = new MailMessage();
            server = new SmtpClient(serverPath);

        }
        public SendEmail(string s, string mf, string pwd, string mt, string sb, string bo, int p, MailMessage mm, SmtpClient sv)
        {
            this.serverPath = s;
            this.mailFrom = mf;
            this.password = pwd;
            this.mailTo = mt;
            this.subject = sb;
            this.body = bo;
            this.port = p;
            this.mail = mm;
            this.server = sv;
        }
        public string ServerPath
        {
            get { return serverPath; }
            set { serverPath = value; }
        }
        public string MailFrom
        {
            get { return mailFrom; }
            set { mailFrom = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string MailTo
        {
            get { return mailTo; }
            set { mailTo = value; }
        }
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
        public string Body
        {
            get { return body; }
            set { body = value; }
        }
        public int Port
        {
            get { return port; }
            set { port = value; }
        }
        public SmtpClient Server
        {
            get { return server; }
            set { server = value; }
        }
        public MailMessage Mail
        {
            get { return mail; }
            set { mail = value; }
        }

        public void SendAlarm()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(serverPath);

                mail.From = new MailAddress(mailFrom);
                mail.To.Add(mailTo);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = port;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}




