using System.Web.Mvc;
using Framework.Logging;

namespace Framework.Common.Web.Filters.Mvc
{
    public class LogExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public LogExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            _logger.Log(filterContext.Exception);
        }
    }
}