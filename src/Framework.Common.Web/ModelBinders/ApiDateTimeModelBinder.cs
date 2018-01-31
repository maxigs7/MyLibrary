using System;
using System.Globalization;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Framework.Common.Web.ModelBinders
{
    /// <summary>
    /// Binds datetime values from api requests to model
    /// </summary>
    public class ApiDateTimeBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var date = value?.ConvertTo(typeof(DateTime?), CultureInfo.CurrentCulture) as DateTime?;
            if (date != null)
            {
                bindingContext.Model = date.Value;
            }

            return true;
        }
    }
}
