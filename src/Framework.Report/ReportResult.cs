using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Report.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Framework.Report
{
    public class ReportResult
    {
        public ReportResult()
        {
            
        }

        public byte[] Report { get; set; }

        public string MimeType { get; set; }

        public ReportResult AddWatermark(string text)
        {
            var font = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            return AddWatermark(text, font);
        }

        public ReportResult AddWatermark(string text, BaseFont baseFont)
        {
            BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Report = PdfHelper.AddWatermark(Report, baseFont, text);
            return this;
        }

        public ReportResult Rotate(int desiredRotation = 90)
        {
            Report = PdfHelper.Rotate(Report, desiredRotation);
            return this;
        }

        public ReportResult AddPageNumber()
        {
            Report = PdfHelper.AddPageNumbers(Report);
            return this;
        }
    }
}
