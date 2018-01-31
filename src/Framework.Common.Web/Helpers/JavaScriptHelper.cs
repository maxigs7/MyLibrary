using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Framework.Common.Web.Helpers
{
    public static class JavaScriptHelper
    {
        public static IHtmlString Json(this HtmlHelper helper, object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //Converters = new JsonConverter[]
                //{
                //    new StringEnumConverter(),
                //},
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            };

            return MvcHtmlString.Create(JsonConvert.SerializeObject(obj, settings));
        }

        public static IHtmlString AngularHiddenFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string prefix)
        {
            //helper.ViewData.TemplateInfo.HtmlFieldPrefix = prefix;
            return helper.EditorFor(expression, "Angular/Hidden", new { Prefix = prefix });
        }

        public static IHtmlString AngularEditorFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string modelPrefix)
        {
            return helper.AngularEditorFor(expression, new { Prefix = modelPrefix });
        }

        public static IHtmlString AngularEditorFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, object viewData)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            return helper.EditorFor(expression, AngularTemplateHelper.GetTemplateForProperty(metadata), viewData);
        }

        public static IHtmlString AngularDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, string modelPrefix)
        {
            return helper.AngularDisplayFor(expression, new { Prefix = modelPrefix });
        }

        public static IHtmlString AngularDisplayFor<TModel, TValue>(this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TValue>> expression, object viewData)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);
            return helper.DisplayFor(expression, AngularTemplateHelper.GetTemplateForProperty(metadata), viewData);
        }

        public static IHtmlString AngularDisplayForModel(this HtmlHelper helper,
            string modelPrefix)
        {
            return AngularDisplayForModel(helper, "Angular/Object", modelPrefix);
        }

        public static IHtmlString AngularDisplayForModel(this HtmlHelper helper,
            string templateName, string modelPrefix)
        {
            return helper.DisplayForModel(templateName,
                new { Prefix = modelPrefix });
        }

        public static IHtmlString AngularEditorForModel(this HtmlHelper helper,
            string modelPrefix)
        {
            return AngularEditorForModel(helper, "Angular/Object", modelPrefix);
        }

        public static IHtmlString AngularEditorForModel(this HtmlHelper helper,
            string templateName, string modelPrefix)
        {
            return AngularEditorForModel(helper, templateName,
                new { Prefix = modelPrefix });
        }

        public static IHtmlString AngularEditorForModel(this HtmlHelper helper,
            string templateName, object viewData)
        {
            return helper.EditorForModel(templateName,
                viewData);
        }

        public static IHtmlString AngularBindingForModel(this HtmlHelper helper, string modelName)
        {
            var prefix = (string)(helper.ViewBag.Prefix);

            if (prefix != null)
            {
                prefix = prefix + ".";
            }

            return MvcHtmlString.Create(prefix + CamelCaseIdForModel(modelName));
        }

        public static IHtmlString AngularBindingForModel(this HtmlHelper helper)
        {
            var prefix = (string)(helper.ViewBag.Prefix);

            if (!string.IsNullOrEmpty(prefix))
            {
                prefix = prefix + ".";
            }

            return MvcHtmlString.Create(prefix + helper.CamelCaseIdForModel());
        }

        //Adapted from JSON.NET.
        public static string CamelCaseIdForModel(this HtmlHelper helper)
        {
            var input = helper.IdForModel().ToString();
            var names = input.Split('_');
            var strBuilder = new StringBuilder();
            for (int i = 0; i < names.Length; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append('.');
                }
                strBuilder.Append(CamelCaseIdForModel(names[i]));
            }
            return strBuilder.ToString();
        }


        public static IHtmlString AngularPreBindJson(this HtmlHelper helper, string model)
        {
            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            // Create tag builder
            var builder = new TagBuilder("pre");

            // Add attributes
            builder.MergeAttribute("data-ng-bind", model + " | json");

            // Render tag
            return MvcHtmlString.Create(builder.ToString());
        }

        private static string CamelCaseIdForModel(string input)
        {
            if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            {
                return input;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < input.Length; ++i)
            {
                var flag = i + 1 < input.Length;
                if (i == 0 || !flag || char.IsUpper(input[i + 1]))
                {
                    var ch = char.ToLower(input[i], CultureInfo.InvariantCulture);
                    sb.Append(ch);
                }
                else
                {
                    sb.Append(input.Substring(i));
                    break;
                }
            }

            return sb.ToString();
        }
    }
}