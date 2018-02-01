using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Framework.Report.Helpers;

namespace Framework.Report.Factory
{
    public class ReportFactory
    {
        /// <summary>
        /// Page width in cm
        /// </summary>
        public decimal? PageWidth { get; private set; }

        /// <summary>
        /// Page height in cm
        /// </summary>
        public decimal? PageHeight { get; private set; }

        /// <summary>
        /// Margin Top in cm
        /// </summary>
        public decimal? MarginTop { get; private set; }

        /// <summary>
        /// Margin Left in cm
        /// </summary>
        public decimal? MarginLeft { get; private set; }

        /// <summary>
        /// Margin Right in cm
        /// </summary>
        public decimal? MarginRight { get; private set; }

        /// <summary>
        /// Margin Bottom in cm
        /// </summary>
        public decimal? MarginBottom { get; private set; }

        /// <summary>
        /// Report Path (RDL or RDLC)
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// DataSets for Report
        /// </summary>
        public Dictionary<string, object> DataSets { get; private set; }

        /// <summary>
        /// Paramaters and values for report
        /// </summary>
        public Dictionary<string, string> Parameters { get; private set; }

        /// <summary>
        /// Images for report
        /// </summary>
        public Dictionary<string, string> Images { get; private set; }

        public List<string> ColumnsToShow { get; private set; }

        /// <summary>
        /// MimeType after render.
        /// </summary>
        public String MimeType { get; private set; }

        public ReportFactory()
        {
            this.DataSets = new Dictionary<string, object>();
            this.Parameters = new Dictionary<string, string>();
            this.Images = new Dictionary<string, string>();
            this.ColumnsToShow = new List<string>();
        }

        public ReportFactory SetFullPath(string fullPath)
        {
            this.FullPath = fullPath;
            return this;
        }

        public ReportFactory SetDataSet(Dictionary<string, object> dataSources)
        {
            foreach (var clave in dataSources.Keys)
            {
                SetDataSet(clave, dataSources[clave]);
            }

            return this;
        }

        public ReportFactory SetDimencines(decimal pageWidth, decimal pageHeight, decimal marginTop, decimal marginLeft, decimal marginRight, decimal marginBottom)
        {
            this.PageWidth = pageWidth;
            this.PageHeight = pageHeight;
            this.MarginTop = marginTop;
            this.MarginLeft = marginLeft;
            this.MarginRight = marginRight;
            this.MarginBottom = marginBottom;
            return this;
        }

        /// <summary>
        /// Add a dataset for the report.
        /// </summary>
        /// <param name="dataSetName">Name of dataset</param>
        /// <param name="dataSet">Content for dataset</param>
        /// <returns></returns>
        public ReportFactory SetDataSet(string dataSetName, object dataSet)
        {
            if (String.IsNullOrEmpty(dataSetName))
            {
                throw new ApplicationException("Must specify a name for dataset");
            }

            if (this.DataSets.ContainsKey(dataSetName))
            {
                this.DataSets[dataSetName] = dataSet;
            }
            else
            {
                this.DataSets.Add(dataSetName, dataSet);
            }

            return this;
        }

        /// <summary>
        /// Add a parameter to report. If the parameter exists, it will be overwritten
        /// </summary>
        /// <param name="param">Parameter dictionary</param>
        /// <returns></returns>
        public ReportFactory SetParameter(Dictionary<string, string> param)
        {
            foreach (var clave in param.Keys)
            {
                SetParameter(clave, param[clave]);
            }

            return this;
        }

        /// <summary>
        /// Add a parameter to report. If the parameter exists, it will be overwritten
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public ReportFactory SetParameter(string key, string value)
        {

            if (String.IsNullOrEmpty(key))
            {
                throw new ApplicationException("No se puede enviar un parametro sin nombre al reporte");
            }

            if (this.Parameters.ContainsKey(key))
            {
                this.Parameters[key] = value;
            }
            else
            {
                this.Parameters.Add(key, value);
            }

            return this;
        }

        public ReportFactory SetImages(Dictionary<string, string> images)
        {
            foreach (var key in images.Keys)
            {
                SetParameter(key, images[key]);
            }

            return this;
        }

        public ReportFactory SetImages(string key, string imagePath)
        {

            if (String.IsNullOrEmpty(imagePath))
            {
                throw new ApplicationException("Can't assign an empty path");
            }

            if (!File.Exists(imagePath))
            {
                throw new ApplicationException("The image does not exist");
            }

            var bitmap = new Bitmap(imagePath);
            string imageBase64;
            //TODO vericar formato de imagen 
            var format = ImageFormat.Png;
            var formatExtension = "png";

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, format);
                imageBase64 = Convert.ToBase64String(ms.ToArray());
            }

            //Check if the image is loaded
            if (this.Images.ContainsKey(key))
            {
                this.Images[key] = imageBase64;
            }
            else
            {
                this.Images.Add(key, imageBase64);
            }

            return this;
        }

        /// <summary>
        /// Add columns to show
        /// </summary>
        /// <param name="columnsToShow"></param>
        /// <returns></returns>
        /// 
        public ReportFactory SetColumnsToShow(List<string> columnsToShow)
        {

            foreach (var column in columnsToShow)
            {
                SetColumnsToShow(column);
            }

            return this;
        }

        public ReportFactory SetColumnsToShow(string[] columnsToShow)
        {
            SetColumnsToShow(columnsToShow.ToList());

            return this;
        }

        /// <summary>
        /// Add columns to show
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public ReportFactory SetColumnsToShow(string column)
        {

            //Check if a column exists
            if (!this.ColumnsToShow.Contains(column))
            {
                this.ColumnsToShow.Add(column);
            }

            return this;
        }

        /// <summary>
        /// Set columns to show since a csv string
        /// </summary>
        /// <param name="stringSeparated"></param>
        /// <returns></returns>
        public ReportFactory SetColumnsToShowSinceCsv(string stringSeparated)
        {
            var columnas = stringSeparated.Split(',');
            return SetColumnsToShow(columnas);
        }

        #region Render

        public ReportResult Render(ReportTypeEnum type)
        {
            var mimeTypeOut = string.Empty;
            var file = ReportHelper.RenderReport(
                type,
                this.DataSets,
                this.FullPath.ToString(),
                this.ColumnsToShow.ToArray(),
                this.Parameters,
                this.Images, this.PageWidth, this.PageHeight, this.MarginTop, this.MarginLeft, this.MarginRight,
                this.MarginBottom,
                out mimeTypeOut);

            this.MimeType = mimeTypeOut;
            var reportResult = new ReportResult()
            {
                Report = file,
                MimeType = mimeTypeOut
            };
            return reportResult;
        }

        #endregion
    }
}
