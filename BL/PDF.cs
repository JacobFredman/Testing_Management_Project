using System.Diagnostics;
using BE.MainObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using static PdfSharp.Drawing.XStringFormat;

namespace BL
{
    public class Pdf
    {
      // PdfDocument document;

        public static void CreateLicensePdf(Test test, Trainee trainee)
        {
            string traineeFullName = trainee.FirstName + trainee.LastName;
            CreateDocument(traineeFullName, trainee.Id, test.ActualTestTime.ToShortDateString(), test.LicenseType.ToString());
        }

        private static void CreateDocument(string fullName, uint id, string testDate, string type) // need to be privete
        {
           PdfDocument  document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var title = new XFont("Verdana", 20, XFontStyle.Bold);
            var font = new XFont("Verdana", 11, XFontStyle.Bold);

      
           gfx.DrawString("Temporary License", title, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), format:XStringFormats.TopCenter);
      
            gfx.DrawString("Full name: " +fullName , font, XBrushes.Black, new XRect(0, -300, page.Width, page.Height), format: XStringFormats.Center);
            gfx.DrawString("Id: " + id.ToString(), font, XBrushes.Black, new XRect(0, -280, page.Width, page.Height), format: XStringFormats.Center);
            gfx.DrawString("Test date: " + testDate, font, XBrushes.Black, new XRect(0, -260, page.Width, page.Height), format: XStringFormats.Center);
            gfx.DrawString("license Type: " + type, font, XBrushes.Black, new XRect(0, -240, page.Width, page.Height), format: XStringFormats.Center);


            string filename = "C:/Users/user/Source/Repos/Project01_5997_2519_dotNet5779/documents/license.pdf";
            document.Save(filename);
          //  Process.Start(filename);
        }

    }
}
