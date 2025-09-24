using Microsoft.Extensions.Logging;
using System;
using NLogLevel = NLog.LogLevel;
using MSLogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace eqconsole
{
    public class NLogLoggerAdapter : ILogger
    {
        private readonly NLog.Logger _logger;
        private readonly string _categoryName;

        public NLogLoggerAdapter(string categoryName = "GameClient")
        {
            _categoryName = categoryName;
            _logger = NLog.LogManager.GetLogger(categoryName);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NLog.ScopeContext.PushProperty("Scope", state);
        }

        public bool IsEnabled(MSLogLevel logLevel)
        {
            return _logger.IsEnabled(ConvertToNLogLevel(logLevel));
        }

        public void Log<TState>(MSLogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var nlogLevel = ConvertToNLogLevel(logLevel);
            var message = formatter(state, exception);
            
            if (exception != null)
            {
                _logger.Log(nlogLevel, exception, message);
            }
            else
            {
                _logger.Log(nlogLevel, message);
            }
        }

        private static NLogLevel ConvertToNLogLevel(MSLogLevel logLevel)
        {
            return logLevel switch
            {
                MSLogLevel.Trace => NLogLevel.Trace,
                MSLogLevel.Debug => NLogLevel.Debug,
                MSLogLevel.Information => NLogLevel.Info,
                MSLogLevel.Warning => NLogLevel.Warn,
                MSLogLevel.Error => NLogLevel.Error,
                MSLogLevel.Critical => NLogLevel.Fatal,
                MSLogLevel.None => NLogLevel.Off,
                _ => NLogLevel.Info
            };
        }
    }

    public class NLogLoggerAdapter<T> : NLogLoggerAdapter, ILogger<T>
    {
        public NLogLoggerAdapter() : base(typeof(T).Name)
        {
        }
    }
}