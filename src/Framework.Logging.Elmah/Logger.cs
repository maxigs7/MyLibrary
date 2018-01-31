using System;
using System.Diagnostics;
using Elmah;
using ApplicationException = Elmah.ApplicationException;

namespace Framework.Logging.Elmah
{
    /// <summary>
    /// ILogger implementation using Elmah.
    /// </summary>
    public class Logger : ILogger
    {
        public void Log(LogLevel logLevel, Exception exception, Type loggingType, bool notifyAdministrator, string stringFormat, params object[] args)
        {
            Log(exception);
        }

        public void Log(Exception exception)
        {
            // Log to Tracing.
            Trace.TraceError(exception.ToString());
            // Log to Elmah.
            ErrorSignal.FromCurrentContext().Raise(exception);
        }

        public void Log(string message)
        {
            Log(new ApplicationException(message));
        }

        public void LogError(string message, Exception ex)
        {
            Log(new ApplicationException(message, ex));
        }
    }
}
