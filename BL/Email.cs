using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using BE;
using BE.MainObjects;

namespace BL
{
    public static class Email
    {
        private const string FromEmailAddress = "tests.miniproject@gmail.com";
        private const string SenderPassword = "0586300016";

        public static int SendEmailToAllTraineeBeforeTest(this IEnumerable<Test> tests)
        {
            var count = 0;
            foreach (var test in tests)
            {
                var trainee = FactoryBl.GetObject.AllTrainees.First(x => x.Id == test.TraineeId);
                try
                {
                    SentEmailToTraineeBeforeTest(test, trainee);
                    count++;
                }
                catch
                {
                }
            }

            return count;
        }

        public static int SendEmailToAllTraineeAfterTest(this IBL bl)
        {
            var count = 0;
            foreach (var test in bl.GetAllTestsThatHappened())
            {
                var trainee = bl.AllTrainees.First(x => x.Id == test.TraineeId);
                try
                {
                    Pdf.CreateLicensePdf(test, trainee);
                    SentEmailToTraineeAfterTest(test, trainee);
                }
                catch (Exception ex)
                {
                    var ms = ex.Message;
                }
            }

            return count;
        }


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

        public static void SentEmailToTraineeAfterTest(Test test, Trainee trainee)
        {
            var subject = test.Passed == true
                ? "Congratulations for the new license"
                : "We are sorry to inform you that you didn't Passed the test";
            var message = (test.Passed == true
                ? "You successfully passed in the test on " + test.ActualTestTime.ToString("f") + ". \nYou are allowed now to drive.\nWe sent a temporary license in the attachment."
                : "You have  do the test again.");
            SentEmail(trainee.EmailAddress, subject, message, trainee.FirstName + " " + trainee.LastName, "D.M.V",test.Passed==true);
        }

        public static void SentEmailToTraineeBeforeTest(Test test, Trainee trainee)
        {
            var subject = trainee.FirstName + " " + trainee.LastName + ", you have a test today";
            var message = trainee.FirstName + " " + trainee.LastName + ", are you prepared for test ?\n\nTest location: " +
                          test.AddressOfBeginningTest +".\nTest Time: "+test.TestTime.ToString("t")+ ".";
            SentEmail(trainee.EmailAddress, subject, message, trainee.FirstName + " " + trainee.LastName, "D.M.V");
        }

        private static void SentEmail(string toAddress, string subject, string bodyMessage, string toName,
            string fromName,bool SentAtt=false)
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
                Credentials = new NetworkCredential(from.Address, fromPassword)
            };


            var message = new MailMessage(from, to)
            {
                Subject = subject,
                Body = bodyMessage
            };


            if (SentAtt)
            {
                Attachment attachment;
                attachment =
                    new Attachment(Configuration.GetPdfFullPath());
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
                attachment.Dispose();
            }
            else
            {
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

            message.Dispose();
            
        }
    }
}