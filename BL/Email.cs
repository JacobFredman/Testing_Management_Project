using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using BE.MainObjects;
using System.Linq;
using BE;
using System.ComponentModel.DataAnnotations;

namespace BL
{
    public static class Email
    {

        /// <summary>
        ///  extension method for test class
        /// </summary>
        /// <param name="tests"></param>
        /// <returns>the number of emails sent</returns>
        public static int SendEmailToAllTraineeBeforeTest(this IEnumerable<Test> tests)
        {
            var count = 0;
            foreach (var test in tests)
            {
                var trainee = FactoryBl.GetObject.AllTrainees.First(x => x.Id == test.TraineeId);
                try
                {
                    SendEmailToTraineeBeforeTest(test, trainee);
                    count++;
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            return count;
        }

        /// <summary>
        /// inform the trainee if he passed the test, and sends him temporary license if he did
        /// </summary>
        /// <param name="test">the test object</param>
        /// <param name="trainee">the trainee who took the test</param>
        public static void SentEmailToTraineeAfterTest(Test test, Trainee trainee)
        {

            var subject = test.Passed == true
                       ? "Congratulations for your new license"
                       : "We are sorry to inform you that you didn't pass the test this time";
            var message = test.Passed == true
                ? "You successfully passed the test on " + test.ActualTestTime.ToString("yyyy - MM - dd") + ", now you are allowed to drive"
                : "you have to take the test again";
            var addAttachment = test.Passed == true;
            SentEmail(addAttachment, trainee.EmailAddress, subject, message, trainee.FirstName + " " + trainee.LastName, "D.M.V");
        }


        /// <summary>
        /// a function that sends an email to trainee in order to remind him about the test
        /// </summary>
        /// <param name="test"></param>
        /// <param name="trainee">trainee to send</param>
        private static void SendEmailToTraineeBeforeTest(Test test, Trainee trainee)
        {
            var subject = trainee.FirstName + ", Reminder: Don't forget your test today";
            var message = trainee.FirstName + ", Are you prepared for test already?" + " starting point is: " +
                          test.AddressOfBeginningTest + ". for more details please see the attached form";
            SentEmail(false, trainee.EmailAddress, subject, message, trainee.FirstName + " " + trainee.LastName, "D.M.V");
        }

        /// <summary>
        ///  sends an email
        /// </summary>
        /// <param name="addAttachment">a pdf file with the new license</param>
        /// <param name="toAddress">the email address of the addressee</param>
        /// <param name="subject">subject</param>
        /// <param name="bodyMessage">bodyMessage</param>
        /// <param name="toName">the addressee name</param>
        /// <param name="fromName">the sender name</param>
        private static void SentEmail(bool addAttachment, string toAddress, string subject, string bodyMessage, string toName,
            string fromName)
        {
            if (!new EmailAddressAttribute().IsValid(toAddress))
                throw new Exception("Please Add a Valid Email To Trainee.");

            var attachment = new Attachment(Configuration.GetPdfFullPath());

            var from = new MailAddress(Configuration.FromEmailAddress, fromName);
            var to = new MailAddress(toAddress, toName);
            const string fromPassword = Configuration.SenderPassword;

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

            try
            {
                if (addAttachment)
                    message.Attachments.Add(attachment);

                smtp.Send(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            attachment.Dispose();
            message.Dispose();
        }

    }

}




