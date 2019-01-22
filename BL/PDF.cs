using System.IO;
using BE;
using BE.MainObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BL
{
    /// <summary>
    ///     creates a pdf file
    /// </summary>
    public static class Pdf
    {
        /// <summary>
        ///     create a temporary license pdf
        /// </summary>
        /// <param name="test">the passed test</param>
        /// <param name="trainee">the passed trainee</param>
        public static void CreateLicensePdf(Test test, Trainee trainee)
        {
            var traineeFullName = trainee.FirstName + trainee.LastName;
            CreateDocument(traineeFullName, trainee.Id, test.ActualTestTime.ToShortDateString(),
                test.LicenseType.ToString());
        }

        /// <summary>
        ///     create a pdf document by parameters
        /// </summary>
        /// <param name="fullName">the trainee full name</param>
        /// <param name="traineeId">traineeId</param>
        /// <param name="testDate">the date when the test was</param>
        /// <param name="licenseType">licenseType</param>
        private static void CreateDocument(string fullName, uint traineeId, string testDate, string licenseType)
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var title = new XFont("Verdana", 20, XFontStyle.Bold);
            var font = new XFont("Verdana", 11, XFontStyle.Bold);


            gfx.DrawString("Temporary License", title, XBrushes.Black, new XRect(0, 0, page.Width, page.Height),
                XStringFormats.TopCenter);

            gfx.DrawString("Full name: " + fullName, font, XBrushes.Black, new XRect(0, -300, page.Width, page.Height),
                XStringFormats.Center);
            gfx.DrawString("Id: " + traineeId, font, XBrushes.Black, new XRect(0, -280, page.Width, page.Height),
                XStringFormats.Center);
            gfx.DrawString("Test date: " + testDate, font, XBrushes.Black, new XRect(0, -260, page.Width, page.Height),
                XStringFormats.Center);
            gfx.DrawString("license Type: " + licenseType, font, XBrushes.Black,
                new XRect(0, -240, page.Width, page.Height),
                XStringFormats.Center);

            // if another temporary license exists delete
            if (File.Exists(Configuration.GetPdfFullPath()))
                File.Delete(Configuration.GetPdfFullPath());

            //save
            document.Save(Configuration.GetPdfFullPath());
            document.Close();
        }
    }
}