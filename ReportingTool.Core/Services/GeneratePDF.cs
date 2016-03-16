using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SelectPdf;

namespace ReportingTool.Core.Services
{
   public static class GeneratePDF
    {
        public static void Generate()
        {
            HtmlToPdf converter = new HtmlToPdf();

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl("http://selectpdf.com");

            // save pdf document
            doc.Save("Sample.pdf");

            // close pdf document
            doc.Close();
        }
    }
}
