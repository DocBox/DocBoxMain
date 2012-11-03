using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Net.Mail;
using System.Net;

namespace docbox.Utilities
{
    public class EmailMessaging
    {

        
        static System.Configuration.Configuration webConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("/docbox/web.config");
        static System.Configuration.KeyValueConfigurationElement hostEmailId =
                    webConfig.AppSettings.Settings["adminEmail"];
        static System.Configuration.KeyValueConfigurationElement hostPassword =
                   webConfig.AppSettings.Settings["adminEmailPassword"];

        
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
         
        }
        public static void sendMessage(string toEmialId, string emailBody, string emailSubject)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credentials = new NetworkCredential(hostEmailId.Value, hostPassword.Value);
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            client.EnableSsl = true;
            MailAddress from = new MailAddress(hostEmailId.Value+"@gmail.com",
               "Docbox" + (char)0xD8 + " Administrator",
            System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(toEmialId);
            
            MailMessage message = new MailMessage(from, to);
            message.Body = emailBody;
            
            message.Body += Environment.NewLine;
            message.Body += "-------------------------------------------------------------------------------------------------------";
            message.Body += "This email is being sent as part of CSE 545 Software Security Project and is part of testing experiment" + Environment.NewLine;
                          
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = emailSubject ;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
             try
             {
                 client.Send(message);
             }
             catch (Exception ex)
             {
                 Console.WriteLine("Exception is:" + ex.ToString());
             }
             message.Dispose();
           }       
    }
}