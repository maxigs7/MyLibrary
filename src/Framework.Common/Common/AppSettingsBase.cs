using System;
using System.Configuration;

namespace Framework.Common
{
    public abstract class AppSettingsBase : IAppSettingsBase
    {
        public bool EnableOptimizations
        {
            get
            {
                bool type;
                var value = ConfigurationManager.AppSettings["EnableOptimizations"];

                if (bool.TryParse(value, out type))
                {
                    return type;
                }

                return false;
            }
        }

        public string PathBase
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

    }
}