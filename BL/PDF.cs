using System.Diagnostics;
using BE.MainObjects;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using static PdfSharp.Drawing.XStringFormat;

namespace BL
{
    public class Pdf
    {
        public void CreateLicensePdf(Test test, Trainee trainee)
        {
           
        }

        private static void CreateDocument()
        {
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);
            var font = new XFont("Verdana", 20, XFontStyle.Bold);

            if (Center != null)
            {
                gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height),
                    format: Center);
            }

            const string filename = "HelloWorld.pdf";
            document.Save(filename);
            Process.Start(filename);
        }
    }
}
