using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;

namespace Framework.Report.Helpers
{
    public class ReportHelper
    {
        public static byte[] RenderReport(ReportTypeEnum type, Dictionary<string, object> dataSets, string fullPath, string[] columnsToShow,
                                                 IDictionary<string, string> parameters, IDictionary<string, string> images,
                                                 decimal? pageWidth, decimal? pageHeight, decimal? marginTop, decimal? marginLeft, decimal? marginRight, decimal? marginBottom, out string mimeType)
        {
            var localReport = new LocalReport();

            ReporteLoadReportDefinition(localReport, fullPath, images);

            localReport.DataSources.Clear();

            foreach (var dataSet in dataSets.Keys)
            {
                var reportDataSource = new ReportDataSource(dataSet, dataSets[dataSet]);
                localReport.DataSources.Add(reportDataSource);
            }

            //Imagenes externas
            localReport.EnableExternalImages = true;

            if (columnsToShow.Any())
            {
                var sb = new StringBuilder();
                foreach (var columna in columnsToShow)
                {
                    sb.AppendFormat("#{0}#", columna.Trim());
                }
                parameters.Add("Columns", sb.ToString());
            }

            foreach (var clave in parameters.Keys)
            {
                var param = new ReportParameter();
                param.Name = clave;
                param.Values.Add(parameters[clave]);
                localReport.SetParameters(param);
            }

            var outputFormat = OutputFormat(type);
            var reporteType = ReportType(type);
            string encoding;
            string fileNameExtension;

            var deviceInfo = new StringBuilder();

            deviceInfo.AppendFormat("<DeviceInfo>");
            deviceInfo.AppendFormat("<OutputFormat>{0}</OutputFormat>", outputFormat);

            
            if (pageWidth.HasValue)
            {
                deviceInfo.AppendFormat("  <PageWidth>{0}cm</PageWidth>", pageWidth);
            }

            if (pageHeight.HasValue)
            {
                deviceInfo.AppendFormat("  <PageHeight>{0}cm</PageHeight>", pageHeight);
            }

            if (marginTop.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginTop>{0}cm</MarginTop>", marginTop);
            }

            if (marginLeft.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginLeft>{0}cm</MarginLeft>", marginLeft);
            }

            if (marginRight.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginRight>{0}cm</MarginRight>", marginRight);
            }

            if (marginBottom.HasValue)
            {
                deviceInfo.AppendFormat("  <MarginBottom>{0}cm</MarginBottom>", marginBottom);
            }

            deviceInfo.AppendLine("<Encoding>UTF-8</Encoding>");
            deviceInfo.AppendFormat("</DeviceInfo>");

            Warning[] warnings;
            string[] streams;

            localReport.Refresh();

            //Render the report
            return localReport.Render(
                reporteType,
                deviceInfo.ToString(),
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
        }

        /// <summary>
        /// Render un reporte a un type especifico
        /// </summary>
        /// <param name="dataSourceName"></param>
        /// <param name="dataSource"></param>
        /// <param name="fullPath">Path completo para cargar el reporte desde el servidor</param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static byte[] RenderReport(ReportTypeEnum type, Dictionary<string, object> dataSets, string fullPath, string[] columnsToShow,
                                                    IDictionary<string, string> parameters, IDictionary<string, string> images, out string mimeType)
        {
            return RenderReport(type, dataSets, fullPath, columnsToShow, parameters, images, null, null, null, null, null, null, out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum type, string dataSetName, object dataSet, string fullPath,
            string[] columnsToShow, IDictionary<string, string> parameters, out string mimeType)
        {

            var dataSources = new Dictionary<string, object>();
            dataSources.Add(dataSetName, dataSet);

            return RenderReport(type, dataSources, fullPath, columnsToShow, parameters, null, out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum type, string dataSetName, object dataSet, string fullPath, string[] columnsToShow, out string mimeType)
        {
            return RenderReport(type, dataSetName, dataSet, fullPath, columnsToShow, new Dictionary<string, string>(), out mimeType);
        }

        public static byte[] RenderReport(ReportTypeEnum type, string dataSetName, object dataSet, string fullPath, IDictionary<string, string> parameters, out string mimeType)
        {
            return RenderReport(type, dataSetName, dataSet, fullPath, new string[0], parameters, out mimeType);
        }

        /// <summary>
        /// Render un reporte a un type especifico
        /// </summary>
        /// <param name="dataSetName"></param>
        /// <param name="dataSet"></param>
        /// <param name="fullPath">Path completo para cargar el reporte desde el servidor</param>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        public static byte[] RenderReport(ReportTypeEnum type, string dataSetName, object dataSet, string fullPath, out string mimeType)
        {
            return RenderReport(type, dataSetName, dataSet, fullPath, new string[0], new Dictionary<string, string>(), out mimeType);
        }

        /// <summary>
        /// Carga la definicion del reporte cambiando namesapces y secciones 
        /// </summary>
        /// <param name="localReport">Instancia de LocalReport</param>
        /// <param name="fullPath">Path del reporte completo para acceder</param>
        public static void ReporteLoadReportDefinition(LocalReport localReport, string fullPath, IDictionary<string, string> images)
        {
            var strReport = System.IO.File.ReadAllText(fullPath, System.Text.Encoding.Default);
            if (strReport.Contains("http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition"))
            {
                strReport =
                    strReport.Replace(
                        "<Report xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\" xmlns:cl=\"http://schemas.microsoft.com/sqlserver/reporting/2010/01/componentdefinition\" xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2010/01/reportdefinition\">",
                        "<Report xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\" xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition\">");
                strReport =
                    strReport.Replace("<ReportSections>", "").Replace("<ReportSection>", "").Replace(
                        "</ReportSection>", "").Replace("</ReportSections>", "");
            }

            if (images != null)
            {
                foreach (var imageName in images.Keys)
                {
                    strReport = ChangeToEmbeddedImages(strReport, imageName, images[imageName]);
                }
            }

            var bytReport = System.Text.Encoding.UTF8.GetBytes(strReport);
            localReport.LoadReportDefinition(new MemoryStream(bytReport));
        }

        public static string OutputFormat(ReportTypeEnum type)
        {
            var typeCadena = string.Empty;
            switch (type)
            {
                case ReportTypeEnum.Pdf:
                    typeCadena = "PDF";
                    break;
                case ReportTypeEnum.Excel:
                    typeCadena = "Excel";
                    break;
                case ReportTypeEnum.Word:
                    typeCadena = "Word";
                    break;
                case ReportTypeEnum.Imagen:
                    typeCadena = "Image";
                    break;
                case ReportTypeEnum.PNG:
                    typeCadena = "PNG";
                    break;
            }

            return typeCadena;
        }

        public static string ReportType(ReportTypeEnum type)
        {
            var typeString = string.Empty;
            switch (type)
            {
                case ReportTypeEnum.Pdf:
                    typeString = "PDF";
                    break;
                case ReportTypeEnum.Excel:
                    typeString = "Excel";
                    break;
                case ReportTypeEnum.Word:
                    typeString = "Word";
                    break;
                case ReportTypeEnum.Imagen:
                    typeString = "Image";
                    break;
                case ReportTypeEnum.PNG:
                    typeString = "Image";
                    break;
            }

            return typeString;
        }

        private static string ChangeToEmbeddedImages(string xml, string imageName, string imageBase64)
        {
            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                var nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("ns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

                var node = doc.SelectSingleNode("/ns:Report/ns:EmbeddedImages", nsMgr);
                if (node != null)
                {
                    //Recorriendo todas las imagenes EmbeddedImages
                    foreach (XmlNode image in node.ChildNodes)
                    {
                        //Preguntar si el nombre de la imagen es (atributo Name)
                        if (image.Attributes["Name"].Value.ToLower() == imageName.ToLower())
                        {
                            //Modificando el valor
                            foreach (XmlNode elem in image.ChildNodes)
                            {
                                //if (elem.Name == "MIMEType")
                                //{
                                //    elem.InnerText = valor;
                                //}

                                if (elem.Name == "ImageData")
                                {
                                    elem.InnerText = imageBase64;
                                }
                            }
                        }

                    }
                }

                var sw = new StringWriter();
                var xtw = new XmlTextWriter(sw);
                doc.WriteTo(xtw);
                return sw.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
                /*
                 * Possible Exceptions:
                 *  System.ArgumentException
                 *  System.ArgumentNullException
                 *  System.InvalidOperationException
                 *  System.IO.DirectoryNotFoundException
                 *  System.IO.FileNotFoundException
                 *  System.IO.IOException
                 *  System.IO.PathTooLongException
                 *  System.NotSupportedException
                 *  System.Security.SecurityException
                 *  System.UnauthorizedAccessException
                 *  System.UriFormatException
                 *  System.Xml.XmlException
                 *  System.Xml.XPath.XPathException
                */
            }
        }

        public static byte[] MergePdfs(IEnumerable<byte[]> inputFiles)
        {
            var outputStream = new MemoryStream();
            var document = new Document();
            var writer = PdfWriter.GetInstance(document, outputStream);
            document.Open();

            foreach (var input in inputFiles)
            {
                var reader = new PdfReader(input);
                for (var i = 1; i <= reader.NumberOfPages; i++)
                {
                    document.SetPageSize(reader.GetPageSizeWithRotation(i));
                    document.NewPage();
                    var page = writer.GetImportedPage(reader, i);

                    var pageRotation = reader.GetPageRotation(i);
                    var pageWidth = reader.GetPageSizeWithRotation(i).Width;
                    var pageHeight = reader.GetPageSizeWithRotation(i).Height;
                    switch (pageRotation)
                    {
                        case 0:
                            writer.DirectContent.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            break;

                        case 90:
                            writer.DirectContent.AddTemplate(page, 0, -1f, 1f, 0, 0, pageHeight);
                            break;

                        case 180:
                            writer.DirectContent.AddTemplate(page, -1f, 0, 0, -1f, pageWidth, pageHeight);
                            break;

                        case 270:
                            writer.DirectContent.AddTemplate(page, 0, 1f, -1f, 0, pageWidth, 0);
                            break;
                    }
                }
            }

            document.Close();

            return outputStream.ToArray();
        }
    }
}
