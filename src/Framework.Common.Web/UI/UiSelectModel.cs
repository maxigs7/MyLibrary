using System.Text;
using System.Web.Mvc;

namespace Framework.Common.Web.UI
{
    public class UiSelectModel : AngularInputModel<dynamic>
    {
        #region Constants

        public const string KeyNgDisabledButton = "NgDisabledButton";
        public const string KeyNgSelect = "NgSelect";
        public const string KeyNgTagging = "NgTagging";
        public const string KeyNgSelectPropertyText = "NgSelectPropertyText";
        public const string KeyNgSelectPropertyValue = "NgSelectPropertyValue";
        public const string KeyNgSelectComplete = "NgSelectComplete";
        public const string KeyNgSelectFilter = "NgSelectFilter";
        public const string KeyNgSearchEnabled = "NgSearchEnabled";
        public const string KeyAllowClear = "AllowClear";
        public const string KeyNgSelectOrderBy = "NgSelectOrderBy";
        public const string KeyNgSelectList = "NgSelectList";
        public const string KeyNgRefreshDelay = "NgRefreshDelay";
        public const string KeyNgRefresh = "NgRefresh";
        public const string KeyNoChoicesText = "NoChoicesText";
        public const string KeyNgUiSelectOptions = "NgUiSelectOptions";
        public const string KeyNgGroupBy = "NgGroupBy";
        public const string KeyFromService = "FromService";

        #endregion

        #region Constructors

        public UiSelectModel(ViewDataDictionary<dynamic> viewData) : base(viewData)
        {
        }

        #endregion

        #region Properties

        public string NgDisabledButton
        {
            get { return (string)ViewData[KeyNgDisabledButton] ?? NgDisabled; }
        }

        public string NgSelect
        {
            get { return (string)ViewData[KeyNgSelect]; }
        }

        public string NgTagging
        {
            get { return (string)ViewData[KeyNgTagging]; }
        }

        public string NgSelectPropertyText
        {
            get { return (string)ViewData[KeyNgSelectPropertyText]; }
        }

        public string NgSelectPropertyValue
        {
            get { return (string)ViewData[KeyNgSelectPropertyValue]; }
        }

        public bool NgSelectComplete
        {
            get
            {
                var value = ViewData[KeyNgSelectComplete];
                if (value is bool && (bool)value)
                {
                    return (bool)value;
                }

                return false;
            }
        }

        public bool FromService
        {
            get
            {
                var value = ViewData[KeyFromService];
                if (value is bool && (bool)value)
                {
                    return (bool)value;
                }

                return false;
            }
        }


        public string NgSelectFilter
        {
            get { return (string)ViewData[KeyNgSelectFilter]; }
        }

        public string NgSearchEnabled
        {
            get { return (string)ViewData[KeyNgSearchEnabled]; }
        }

        public string AllowClear
        {
            get
            {
                if(!string.IsNullOrEmpty((string)ViewData[KeyAllowClear])) 
                    return (string)ViewData[KeyAllowClear];

                return (!ViewData.ModelMetadata.IsRequired).ToString().ToLower();
            }
        }

        public string NgSelectOrderBy
        {
            get { return (string)ViewData[KeyNgSelectOrderBy]; }
        }

        public string NgSelectList
        {
            get { return (string)ViewData[KeyNgSelectList]; }
        }

        public string NgRefreshDelay
        {
            get { return (string)ViewData[KeyNgRefreshDelay]; }
        }

        public string NgRefresh
        {
            get { return (string)ViewData[KeyNgRefresh]; }
        }

        public string NgGroupBy
        {
            get { return (string)ViewData[KeyNgGroupBy]; }
        }

        public string NoChoicesText
        {
            get { return (string)ViewData[KeyNoChoicesText]; }
        }

        public string NgUiSelectOptions
        {
            get { return (string)ViewData[KeyNgUiSelectOptions]; }
        }

        public string NgUiSelectInit
        {
            get
            {
                var str = new StringBuilder();
                str.AppendFormat("{{  list: {0}, options: {1} }}", NgSelectList, NgUiSelectOptions ?? "null");

                return str.ToString();
            }
        }

        public string NgRepeat
        {
            get
            {
                var str = new StringBuilder();
                var item = NgSelectComplete ? "item" : string.Format("item.{0}", NgSelectPropertyValue);

                str.AppendFormat("{0} as item in {1}", item, NgSelectListFinal);
                if (!string.IsNullOrEmpty(NgSelectFilter))
                {
                    str.AppendFormat(" | filter: {0}", NgSelectFilter);
                }

                if (!string.IsNullOrEmpty(NgSelectOrderBy))
                {
                    str.AppendFormat(" | orderBy: {0}", NgSelectOrderBy);
                }
                return str.ToString();
            }
        }

        private string NgSelectListFinal
        {
            get
            {
                if (FromService)
                {
                    return "uiSelectVm.list";
                }
                return NgSelectList;
            }
        }

        #endregion
    }
}