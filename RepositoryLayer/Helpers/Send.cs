using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace RepositoryLayer.Helpers
{
    public class Send
    {
        public string SendingMail(string To, string token)
        {
            //Mail content
            string From = "dishakamble24@gmail.com";
            MailMessage mail = new MailMessage(From, To);
            string mailBody = "Hello User, Your Generated Token : " + token;
            mail.Subject = "Token Generated for Forgot Password";
            mail.Body = mailBody.ToString();
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = false;

            //SMTP client - Built in Class
            //For interacting with only gmail account user
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);//587 - port number
            NetworkCredential networkCredential = new NetworkCredential("dishakamble24@gmail.com", "fwuf eifp nciq qrps");//(email, app password)


            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = networkCredential;

            smtpClient.Send(mail); //all info will transfer
            return To;


        }
    }
}
