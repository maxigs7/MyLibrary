using System.Web.Mvc;

namespace Framework.Common.Web.UI
{
    public class AngularInputModel <T>
    {
        #region Constructors

        public const string KeyNgBlur = "NgBlur";
        public const string KeyNgClick = "NgClick";
        public const string KeyNgChange = "NgChange";
        public const string KeyNgDisabled = "NgDisabled";
        public const string KeyNgReadonly = "NgReadonly";
        public const string KeyNgModel = "NgModel";
        public const string KeyNgClass = "NgClass";
        public const string KeyNgModelOptions = "NgModelOptions";
        
        public AngularInputModel()
        {
            
        }

        public AngularInputModel(ViewDataDictionary<T> viewData)
        {
            ViewData = viewData;
        }

        #endregion

        #region Properties

        public ViewDataDictionary<T> ViewData { get; set; }

        public virtual string NgClass
        {
            get { return (string) ViewData[KeyNgClass]; }
        }

        public virtual string NgModel
        {
            get { return (string) ViewData[KeyNgModel]; }
        }

        public virtual string NgReadonly
        {
            get { return (string) ViewData[KeyNgReadonly]; }
        }

        public virtual string NgDisabled
        {
            get { return (string) ViewData[KeyNgDisabled]; }
        }

        public virtual string NgChange
        {
            get { return (string) ViewData[KeyNgChange]; }
        }

        public virtual string NgClick
        {
            get { return (string) ViewData[KeyNgClick]; }
        }

        public virtual string NgModelOptions
        {
            get { return (string)ViewData[KeyNgModelOptions]; }
        }

        public virtual string NgBlur
        {
            get { return (string) ViewData[KeyNgBlur]; }
        }

        public string Placeholder
        {
            get { return ViewData.ModelMetadata.Watermark; }
        }

        #endregion
    }

   
}