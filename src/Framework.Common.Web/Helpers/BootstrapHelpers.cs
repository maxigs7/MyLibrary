using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Framework.Common.Web.Helpers
{
	public static class BootstrapHelpers
	{
		public static IHtmlString BootstrapLabelFor<TModel, TProp>(
				this HtmlHelper<TModel> helper,
				Expression<Func<TModel, TProp>> property)
		{
			return helper.LabelFor(property, new
			{
				@class = "col-sm-3 control-label"
			});
		}

		public static IHtmlString BootstrapLabel(
				this HtmlHelper helper,
				string propertyName,
                string labelText = "")
		{
			return helper.Label(propertyName, string.IsNullOrEmpty(labelText) ? propertyName : labelText, new

            {
				@class = "col-sm-3 control-label"
            });
		}

        public static IHtmlString BootstrapLabelBlockFor<TModel, TProp>(
                this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProp>> property)
        {
            return helper.LabelFor(property, new
            {
                @class = "control-label"
            });
        }

        public static IHtmlString BootstrapLabelBlock(
                this HtmlHelper helper,
                string propertyName,
                string labelText = "")
        {
            return helper.Label(propertyName, string.IsNullOrEmpty(labelText)? propertyName : labelText, new
            {
                @class = "control-label"
            });
        }
    }
}