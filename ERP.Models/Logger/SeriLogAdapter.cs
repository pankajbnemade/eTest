using Microsoft.Extensions.Logging;
using Serilog.Context;
using Serilog.Events;
using System;

namespace ERP.Models.Logger
{
    public class SeriLogAdapter : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public SeriLogAdapter()
        {
            _logger = Serilog.Log.Logger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Debug => _logger.IsEnabled(LogEventLevel.Debug),
                LogLevel.Information => _logger.IsEnabled(LogEventLevel.Information),
                LogLevel.Warning => _logger.IsEnabled(LogEventLevel.Warning),
                LogLevel.Error => _logger.IsEnabled(LogEventLevel.Error), //made changes for custom error logging.
                LogLevel.Critical => _logger.IsEnabled(LogEventLevel.Fatal), //made changes for custom error logging.
                _ => throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel)),
            };
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            string message;

            if (null != formatter)
            {
                message = formatter(state, exception);
            }
            else
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            switch (logLevel)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;

                case LogLevel.Information:
                    _logger.Information(message, exception);
                    break;

                case LogLevel.Warning:
                    LogContext.PushProperty("Guid", Guid.NewGuid());
                    _logger.Warning(message, exception);
                    break;

                case LogLevel.Error:
                    LogContext.PushProperty("Guid", Guid.NewGuid());
                    _logger.Error(message, exception);
                    break;

                case LogLevel.Critical:
                    LogContext.PushProperty("Guid", Guid.NewGuid());
                    _logger.Fatal(message, exception);
                    break;

                default:
                    _logger.Warning($"Encountered unknown log level {logLevel}", exception);
                    break;
            }
        }
    }
}
