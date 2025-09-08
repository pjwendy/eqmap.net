using System;
using Microsoft.Extensions.Logging;
using NLog;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace eqmap
{
    public class NLogLoggerAdapter : Microsoft.Extensions.Logging.ILogger<OpenEQ.Netcode.GameClient.EQGameClient>
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("EQGameClient");

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(ConvertLogLevel(logLevel));
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            var nlogLevel = ConvertLogLevel(logLevel);

            if (exception != null)
            {
                Logger.Log(nlogLevel, exception, message);
            }
            else
            {
                Logger.Log(nlogLevel, message);
            }
        }

        private static NLog.LogLevel ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Information:
                    return NLog.LogLevel.Info;
                case LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Critical:
                    return NLog.LogLevel.Fatal;
                case LogLevel.None:
                    return NLog.LogLevel.Off;
                default:
                    return NLog.LogLevel.Info;
            }
        }
    }
}