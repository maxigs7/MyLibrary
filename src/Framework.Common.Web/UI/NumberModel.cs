using System.Web.Mvc;

namespace Framework.Common.Web.UI
{
    public class NumberModel : AngularInputModel<dynamic>
    {
        #region Constants

        public const string KeyMin = "Min";
        public const string KeyMax = "Max";
        public const string KeyFormatting = "Formatting";
        public const string KeyDecimals = "Decimals";

        #endregion

        #region Constructors

        public NumberModel(ViewDataDictionary<dynamic> viewData) : base(viewData)
        {
        }

        #endregion

        #region Properties

        public string Min
        {
            get { return (string)ViewData[KeyMin]; }
        }

        public string Max
        {
            get { return (string)ViewData[KeyMax]; }
        }

        public string Formatting
        {
            get { return (string)ViewData[KeyFormatting]?? "true"; }
        }

        public string Decimals
        {
            get { return (string)ViewData[KeyDecimals] ?? "0"; }
        }

        #endregion
    }
}