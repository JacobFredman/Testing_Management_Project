using System;
using System.Net.Mail;
using System.Net;
using BE;

namespace BL
{
   public class SentEmail
    {

        private const string From = "jacov141@gmail.com";
        private const string Password = "";

        //private MailMessage _mail = new MailMessage("jacAndElisha@miniProject.com", "jacov141@gmail.com");
        //private readonly SmtpClient _client = new SmtpClient();

        //public SentEmail()
        //{
        //    _client.Port = 25;
        //    _client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    _client.UseDefaultCredentials = false;
        //    _client.Host = "smtp.gmail.com";
        //    _mail.Subject = "this is a test email.";
        //    _mail.Body = "this is my test email body";
        //    _client.Send(_mail);
        //}



        public SentEmail()
        {
                var fromAddress = new MailAddress("jacov141@gmail.com", "From Name");
                var toAddress = new MailAddress("jacov141@gmail.com", "To Name");
                const string fromPassword = ""; ///////////////////////////////// missing password
                const string subject = "Subject";
                const string body = "Body";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                };

            try
            {
                smtp.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }    
                  
            }
        }

    }




