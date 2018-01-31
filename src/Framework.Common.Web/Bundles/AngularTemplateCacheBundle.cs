using System.Web.Optimization;

namespace Framework.Common.Web.Bundles
{
    public class AngularTemplateCacheBundle : Bundle
    {
        public AngularTemplateCacheBundle(string moduleName, string virtualPath)
                : base(virtualPath, new AngularTemplateCacheTransform(moduleName))
        {

        }
    }
}
