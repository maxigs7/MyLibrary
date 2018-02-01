using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace Framework.Report.Helpers
{
    public class PdfHelper
    {
        #region Watermark

        /// <summary>
        /// This method adds watermark text under pdf content
        /// </summary>
        /// <param name="pdfData">pdf content bytes</param>
        /// <param name="watermarkText">text to be shown as watermark</param>
        /// <param name="font">base font</param>
        /// <param name="fontSize">font sieze</param>
        /// <param name="angle">angle at which watermark needs to be shown</param>
        /// <param name="color">water mark color</param>
        /// <param name="realPageSize">pdf page size</param>
        public static void AddWaterMarkText(PdfContentByte pdfData, string watermarkText, BaseFont font, float fontSize, float angle, BaseColor color, Rectangle realPageSize)
        {
            var gstate = new PdfGState { FillOpacity = 0.35f, StrokeOpacity = 0.3f };
            pdfData.SaveState();
            pdfData.SetGState(gstate);
            pdfData.SetColorFill(color);
            pdfData.BeginText();
            pdfData.SetFontAndSize(font, fontSize);
            var x = (realPageSize.Right + realPageSize.Left) / 2;
            var y = (realPageSize.Bottom + realPageSize.Top) / 2;
            pdfData.ShowTextAligned(Element.ALIGN_CENTER, watermarkText, x, y, angle);
            pdfData.EndText();
            pdfData.RestoreState();
        }

        // <summary>
        /// This method calls another method to add watermark text for each page
        /// </summary>
        /// <param name="bytes">byte array of Pdf</param>
        /// <param name="baseFont">Base font</param>
        /// <param name="watermarkText">Text to be added as watermark</param>
        /// <returns>Pdf bytes array having watermark added</returns>
        public static byte[] AddWatermark(byte[] bytes, BaseFont baseFont, string watermarkText)
        {
            //BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            using (var ms = new MemoryStream())
            {
                using (var reader = new PdfReader(bytes))
                using (var stamper = new PdfStamper(reader, ms))
                {
                    var pages = reader.NumberOfPages;
                    for (var i = 1; i <= pages; i++)
                    {
                        var dc = stamper.GetOverContent(i);
                        AddWaterMarkText(dc, watermarkText, baseFont, 50, 45, BaseColor.GRAY, reader.GetPageSizeWithRotation(i));
                    }
                    stamper.Close();
                }
                return ms.ToArray();
            }
        }

        #endregion

        #region Html

        public static byte[] FromHtml(string html, float margin = 25)
        {
            return FromHtml(html, margin, margin, margin, margin);
        }

        public static byte[] FromHtml(string html, float marginTopBottom, float marginLeftRight)
        {
            return FromHtml(html, marginTopBottom, marginLeftRight, marginTopBottom, marginLeftRight);
        }

        public static byte[] FromHtml(string html, float marginTop, float marginLeftRight, float marginBottom)
        {
            return FromHtml(html, marginTop, marginLeftRight, marginBottom, marginLeftRight);
        }

        public static byte[] FromHtml(string html, float marginTop, float marginRight, float marginBottom, float marginLeft)
        {
            using (var ms = new MemoryStream())
            {
                // 1: create object of a itextsharp document class
                var doc = new Document(PageSize.A4, marginLeft, marginRight, marginTop, marginBottom);
                // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
                var oPdfWriter = PdfWriter.GetInstance(doc, ms);

                // open the document for writing  
                doc.Open();

                //Parse the HTML
                using (var htmlReader = new StringReader(html))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(oPdfWriter, doc, htmlReader);
                }
                doc.Close();
                return ms.ToArray();
            }
        }

        #endregion

        #region Page Numbers

        public static byte[] AddPageNumbers(byte[] filePdf)
        {
            var format = "Pag. | {0}";
            return AddPageNumbers(filePdf, format);
        }

        public static byte[] AddPageNumbers(byte[] filePdf, Font font)
        {
            var format = "Pag. | {0}";
            return AddPageNumbers(filePdf, font, format, false);
        }

        public static byte[] AddPageNumbers(byte[] filePdf, string format)
        {
            var font = FontFactory.GetFont("Arial", 9, Font.NORMAL, BaseColor.BLACK);
            return AddPageNumbers(filePdf, font, format, false);
        }

        public static byte[] AddPageNumbers(byte[] filePdf, Font font, string format, bool differentByPage)
        {
            using (var stream = new MemoryStream())
            {
                var reader = new PdfReader(filePdf);

                using (var stamper = new PdfStamper(reader, stream))
                {
                    var pages = reader.NumberOfPages;
                    for (var i = 1; i <= pages; i++)
                    {
                        if (i == 1 || i == pages) continue;

                        var align = Element.ALIGN_RIGHT;

                        var x = 568f;
                        if(differentByPage && i % 2 != 0)
                        {
                            x = 52f;
                        }
                        var phrase = new Phrase(string.Format(format, i - 1), font);
                        ColumnText.ShowTextAligned(stamper.GetUnderContent(i), align, phrase, x, 15f, 0);
                    }
                }
                return stream.ToArray();
            }
        }

        #endregion

        public static byte[] Rotate(byte[] filePdf, int desiredRot = 90)
        {
            using (var stream = new MemoryStream())
            {
                var reader = new PdfReader(filePdf);
                //var stamper = new PdfStamper(reader, stream);
                using (var stamper = new PdfStamper(reader, stream))
                {
                    var pages = reader.NumberOfPages;
                    for (var i = 1; i <= pages; i++)
                    {
                        var pageDict = reader.GetPageN(i);
                        //var desiredRot = 90; // 90 degrees clockwise from what it is now
                        var rotation = pageDict.GetAsNumber(PdfName.ROTATE);

                        if (rotation != null)
                        {
                            desiredRot += rotation.IntValue;
                            desiredRot %= 360; // must be 0, 90, 180, or 270
                        }
                        pageDict.Put(PdfName.ROTATE, new PdfNumber(desiredRot));
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
