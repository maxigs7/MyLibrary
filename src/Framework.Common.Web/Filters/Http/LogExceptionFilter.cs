using System.Web.Http.Filters;
using Framework.Logging;

namespace Framework.Common.Web.Filters.Http
{
    public class LogExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public LogExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            _logger.Log(actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}
