using System;
using System.Net.Mail;
using System.Net;
using BE.MainObjects;

namespace BL
{
   public class Email
    {

        private const string FromEmailAddress = "tests.miniproject@gmail.com";
        private const string SenderPassword = "0586300016";


      
       

        //private MailMessage _mail = new MailMessage("jacAndElisha@miniProject.com", "jacov141@gmail.com");
        //private readonly SmtpClient _client = new SmtpClient();

        //public EmailAddress()
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
           
            var subject = test.Passed == true ? "Congratulations for the new license"  : "we are sorry to inform you that you didn't Passed the test this time";
            var message = test.Passed == true ? "You successfully passed in the test in " + test.ActualTestTime + ", now you are allowed to drive" : "you have  do the test again";
          SentEmail(trainee.EmailAddress,subject,message,trainee.FirstName + " " + trainee.LastName,"D.M.V");
        }

        public void SentEmailToTraineeBeforeTest(Test test, Trainee trainee)
        {
             string subject = trainee.FirstName + ", you have a test today";
             string message = trainee.FirstName + ", are you prepared for test already?" + "the beginning place is: " + test.AddressOfBeginningTest + ". for more details please look in the attached";
            SentEmail(trainee.EmailAddress,subject,message, trainee.FirstName + " " + trainee.LastName,"D.M.V");
        }

        private static void SentEmail(string toAddress,string subject,string bodyMessage,string toName, string fromName)
        {
            Attachment attachment;
            attachment = new Attachment("C:/Users/user/Source/Repos/Project01_5997_2519_dotNet5779/documents/license.pdf");

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
                Credentials = new NetworkCredential(@from.Address, fromPassword),
            };
            

            var message = new MailMessage(@from, to)
            {
                Subject = subject,
                Body = bodyMessage,
               
            };

            try
            {
                message.Attachments.Add(attachment);
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




