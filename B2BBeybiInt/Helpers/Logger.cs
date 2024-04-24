using System;
using System.Reflection;
using Serilog;

namespace INT.BeybiB2B.Helpers
{
    public class Logger
    {
        private static readonly ILogger _errorLogger;

        static Logger()
        {
            _errorLogger = new LoggerConfiguration()
                .WriteTo.File(Assembly.GetExecutingAssembly().Location + "~/logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public static void LogInfo(string message)
        {
            _errorLogger.Information(message);
        }

        public static void LogError(Exception ex)
        {
            _errorLogger.Error(ex, "Exception");
        }
    }
}
