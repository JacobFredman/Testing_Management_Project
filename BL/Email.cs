using System;
using System.Net.Mail;
using System.Net;
using BE;

namespace BL
{
   public class Email
    {

        private const string FromEmailAddress = "jacov141@gmail.com";
        private const string SenderPassword = ""; // missing password /////////////////////////////

        //private MailMessage _mail = new MailMessage("jacAndElisha@miniProject.com", "jacov141@gmail.com");
        //private readonly SmtpClient _client = new SmtpClient();

        //public Email()
        //{
        //    _client.Port = 25;
        //    _client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    _client.UseDefaultCredentials = false;
        //    _client.Host = "smtp.gmail.com";
        //    _mail.Subject = "this is a test email.";
        //    _mail.Body = "this is my test email body";
        //    _client.Send(_mail);
        //}

        public void SentEmailToTraineeAfterTest(Test test, Trainee trainee)
        {
            var subject = (test.Passed ==true) ? "Congratulations for the new license" : "we are sorry toAddress inform you that you didn't Passed the test this time";
            var message = (test.Passed == true) ? "now you are allowed toAddress drive" : "you have toAddress do the test again";
          SentEmail(trainee.Email,subject,message,trainee.FirstName + " " + trainee.LastName,"D.M.V");
        }

        public void SentEmailToTraineeBeforeTest(Test test, Trainee trainee)
        {
            const string subject = "you have test today";
            const string message = "are you prepared for test already? see details in the attached";
            SentEmail(trainee.Email,subject,message, trainee.FirstName + " " + trainee.LastName,"D.M.V");
        }

        private static void SentEmail(string toAddress,string subject,string bodyMessage,string toName, string fromName)
        {
            var from = new MailAddress(FromEmailAddress, fromName);
            var to = new MailAddress(toAddress, toName);
            const string fromPassword = SenderPassword; 
          //  const string subject = "Subject";
          //  const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(@from.Address, fromPassword)
            };

            var message = new MailMessage(@from, to)
            {
                Subject = subject,
                Body = bodyMessage
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




