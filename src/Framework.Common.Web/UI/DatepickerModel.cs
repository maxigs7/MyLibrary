using System;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace Framework.Common.Web.UI
{
    public class DatepickerModel : AngularInputModel<DateTime?>
    {
        #region Constants

        public const string KeyDateFormat = "DateFormat";
        public const string KeyDatepickerOptions = "DatepickerOptions";
        public const string KeyCustomClass = "CustomClass";
        public const string KeyDateDisabled = "DateDisabled";
        public const string KeyDatepickerMode = "DatepickerMode";
        public const string KeyFormatDay = "FormatDay";
        public const string KeyFormatMonth = "FormatMonth";
        public const string KeyFormatYear = "FormatYear";
        public const string KeyFormatDayHeader = "FormatDayHeader";
        public const string KeyFormatDayTitle = "FormatDayTitle";
        public const string KeyFormatMonthTitle = "FormatMonthTitle";
        public const string KeyInitDate = "InitDate";
        public const string KeyMaxDate = "MaxDate";
        public const string KeyMaxMode = "MaxMode";
        public const string KeyMinDate = "MinDate";
        public const string KeyMinMode = "MinMode";
        public const string KeyMonthColumns = "MonthColumns";
        public const string KeyNgModelOptions = "NgModelOptions";
        public const string KeyShortcutPropagation = "ShortcutPropagation";
        public const string KeyShowWeeks = "ShowWeeks";
        public const string KeyStartingDay = "StartingDay";
        public const string KeyYearRows = "YearRows";
        public const string KeyYearColumns = "YearColumns";

        private static readonly string DefaultFormat =
            Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;

        #endregion

        #region Constructors

        public DatepickerModel(ViewDataDictionary<DateTime?> viewData, DateTime? model) : base(viewData)
        {
            Value = model;
        }

        #endregion

        #region Properties

        public DateTime? Value { get; set; }

        public string ValueString
        {
            get
            {
                var date = (Value.HasValue)
                    ? Value.Value.ToShortDateString()
                    : string.Empty;
                return date;
            }
        }

        public string DateFormat
        {
            get { return (string)ViewData[KeyDateFormat] ?? DefaultFormat; }
        }

        public string DatepickerOptions
        {
            get { return (string)ViewData[KeyDatepickerOptions]; }
        }

        public string DatepickerOptionsFinal
        {
            get
            {
                if (ViewData[KeyDatepickerOptions] == null) return BuildDatepickerOptionsJson();

                return (string)ViewData[KeyDatepickerOptions];
            }
        }

        #endregion

        #region Util Methods 

        public string BuildDatepickerOptionsJson()
        {
            var str = new StringBuilder();

            str.Append("{");
            if(!string.IsNullOrEmpty(MinDate))
                str.AppendFormat("minDate: {0},", MinDate);

            if (!string.IsNullOrEmpty(MaxDate))
                str.AppendFormat("maxDate: {0},", MaxDate);
            
            str.Append("}");

            return str.ToString();
            //var settings = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            //    NullValueHandling = NullValueHandling.Ignore,
            //};
            //var obj = new
            //{
            //    CustomClass,
            //    DateDisabled,
            //    DatepickerMode,
            //    FormatDay,
            //    FormatMonth,
            //    FormatYear,
            //    FormatDayHeader,
            //    FormatDayTitle,
            //    FormatMonthTitle,
            //    InitDate,
            //    MaxDate,
            //    MaxMode,
            //    MinDate,
            //    MinMode,
            //    MonthColumns,
            //    NgModelOptions,
            //    ShortcutPropagation,
            //    ShowWeeks,
            //    StartingDay,
            //    YearRows,
            //    YearColumns
            //};

            //return JsonConvert.SerializeObject(obj, settings);
        }

        #endregion

        #region DatepickerOptions 

        /// <summary>
        /// customClass({ date: date, mode: mode}) 
        /// - An optional expression to add classes based on passing an object with 
        ///   date and current mode properties.
        /// </summary>
        public string CustomClass
        {
            get { return (string)ViewData[KeyCustomClass]; }
        }

        /// <summary>
        /// dateDisabled ({date: date, mode: mode}) 
        /// - An optional expression to disable visible options based on passing an object 
        ///   with date and current mode properties.
        /// </summary>
        public string DateDisabled
        {
            get { return (string)ViewData[KeyDateDisabled]; }
        }

        /// <summary>
        /// datepickerMode C(Default: day)
        /// - Current mode of the datepicker(day|month|year). Can be used to initialize the datepicker in a specific mode.
        /// </summary>
        public string DatepickerMode
        {
            get { return (string)ViewData[KeyDatepickerMode]; }
        }

        /// <summary>
        /// formatDay C(Default: dd) 
        /// - Format of day in month.
        /// </summary>
        public string FormatDay
        {
            get { return (string)ViewData[KeyFormatDay]; }
        }

        /// <summary>
        /// formatMonth C(Default: MMMM) 
        /// - Format of month in year.
        /// </summary>
        public string FormatMonth
        {
            get { return (string)ViewData[KeyFormatMonth]; }
        }

        /// <summary>
        /// formatYear C(Default: yyyy) - 
        /// Format of year in year range.
        /// </summary>
        public string FormatYear
        {
            get { return (string)ViewData[KeyFormatYear]; }
        }

        /// <summary>
        /// formatDayHeader C (Default: EEE) 
        /// - Format of day in week header.
        /// </summary>
        public string FormatDayHeader
        {
            get { return (string)ViewData[KeyFormatDayHeader]; }
        }

        /// <summary>
        /// formatDayTitle C (Default: MMMM yyyy) 
        /// - Format of title when selecting day.
        /// </summary>
        public string FormatDayTitle
        {
            get { return (string)ViewData[KeyFormatDayTitle]; }
        }

        /// <summary>
        /// formatMonthTitle C (Default: yyyy) 
        /// - Format of title when selecting month.
        /// </summary>
        public string FormatMonthTitle
        {
            get { return (string)ViewData[KeyFormatMonthTitle]; }
        }

        /// <summary>
        /// initDate(Default: null) 
        /// - The initial date view when no model value is specified.
        /// </summary>
        public string InitDate
        {
            get { return (string)ViewData[KeyInitDate]; }
        }

        /// <summary>
        /// maxDate C(Default: null) 
        /// - Defines the maximum available date.Requires a Javascript Date object.
        /// </summary>
        public string MaxDate
        {
            get { return (string)ViewData[KeyMaxDate]; }
        }

        /// <summary>
        /// maxMode C(Default: year) 
        /// - Sets an upper limit for mode.
        /// </summary>
        public string MaxMode
        {
            get { return (string)ViewData[KeyMaxMode]; }
        }

        /// <summary>
        /// minDate C(Default: null) 
        /// - Defines the minimum available date.Requires a Javascript Date object.
        /// </summary>
        public string MinDate
        {
            get { return (string)ViewData[KeyMinDate]; }
        }

        /// <summary>
        /// minMode C(Default: day) 
        /// - Sets a lower limit for mode.
        /// </summary>
        public string MinMode
        {
            get { return (string)ViewData[KeyMinMode]; }
        }

        /// <summary>
        /// monthColumns C(Default: 3) 
        /// - Number of columns displayed in month selection.
        /// </summary>
        public string MonthColumns
        {
            get { return (string)ViewData[KeyMonthColumns]; }
        }

        /// <summary>
        /// ngModelOptions C (Default: null) 
        /// - Sets ngModelOptions for datepicker.This can be overridden through attribute usage
        /// </summary>
        public string NgModelOptions
        {
            get { return (string)ViewData[KeyNgModelOptions]; }
        }

        /// <summary>
        /// shortcutPropagation C(Default: false) 
        /// - An option to disable the propagation of the keydown event.
        /// </summary>
        public string ShortcutPropagation
        {
            get { return (string)ViewData[KeyShortcutPropagation]; }
        }

        /// <summary>
        /// showWeeks C (Default: true) - Whether to display week numbers.
        /// </summary>
        public string ShowWeeks
        {
            get { return (string)ViewData[KeyShowWeeks] ?? "false"; }
        }

        /// <summary>
        /// startingDay C (Default: $locale.DATETIME_FORMATS.FIRSTDAYOFWEEK) 
        /// - Starting day of the week from 0-6 (0=Sunday, ..., 6=Saturday).
        /// </summary>
        public string StartingDay
        {
            get { return (string)ViewData[KeyStartingDay]; }
        }

        /// <summary>
        /// yearRows C (Default: 4) 
        /// - Number of rows displayed in year selection.
        /// </summary>
        public string YearRows
        {
            get { return (string)ViewData[KeyYearRows]; }
        }

        /// <summary>
        /// yearColumns C (Default: 5) 
        /// - Number of columns displayed in year selection.
        /// </summary>
        public string YearColumns
        {
            get { return (string)ViewData[KeyYearColumns]; }
        }

        #endregion
    }

}


