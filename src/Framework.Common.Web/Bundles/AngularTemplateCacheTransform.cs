using System.Text;
using System.Web.Optimization;

namespace Framework.Common.Web.Bundles
{
    public class AngularTemplateCacheTransform : IBundleTransform
    {
        private readonly string _moduleName;

        public AngularTemplateCacheTransform(string moduleName)
        {
            _moduleName = moduleName;
        }

        public void Process(BundleContext context, BundleResponse bundleResponse)
        {
            var strBuilder = new StringBuilder();
            // Javascript module for Angular that uses templateCache 
            strBuilder.AppendFormat(
                @"(function() {{" +
                    @"angular.module('{0}')" +
                        @".run(['$templateCache', function($templateCache) {{",
                _moduleName
            );

            foreach (var file in bundleResponse.Files)
            {
                // Get content of file
                var content = file.ApplyTransforms();
                // Remove newlines and replace ' with \\'
                content = content.Replace("'", "\\'").Replace("\r\n", "");
                // Find templateUrl by getting file path and removing inital ~
                var templateUrl = file.IncludedVirtualPath.Replace("~", "").Replace(@"\", "/").ToLower();
                // Add content of template file inside an Angular put method
                strBuilder.AppendFormat("$templateCache.put('{0}','{1}');", templateUrl, content);
            }

            strBuilder.Append(
                    @"}]);" +
                @"})();"
            );

            bundleResponse.Files = new BundleFile[] {};
            bundleResponse.Content = strBuilder.ToString();
            bundleResponse.ContentType = "text/javascript";
        }
    }
}
