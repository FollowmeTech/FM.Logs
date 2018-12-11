using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;

namespace FM
{
    public partial class Log4NetLogger : ILogger
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly string _categoryName;
        private readonly bool _includeCategory;
        private readonly ILoggerRepository _loggerRepository;

        public Log4NetLogger(string name, Func<string, LogLevel, bool> filter, ILoggerRepository loggerRepository)
        {
            _categoryName = string.IsNullOrEmpty(name) ? nameof(Log4NetLogger) : name;
            _filter = filter;
            _loggerRepository = loggerRepository;
        }
        
        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return _filter == null || _filter(_categoryName, logLevel);
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message) && exception != null)
            {
                message = exception.ToString();
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var log = LogManager.GetLogger(_loggerRepository.Name, _categoryName);
            string write2JsonFormat(string level)
            {
                if (_categoryName.Equals("grpc.access")) return message;
                if (_categoryName.Equals("grpc.call")) return message;

                return Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    categoryname = _categoryName,
                    level = level,
                    msg = message,
                    time = DateTime.Now
                });
            }

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                case LogLevel.None:
                    log.Info(write2JsonFormat("info"));
                    break;
                case LogLevel.Warning:
                    log.Warn(write2JsonFormat("warn"));
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    log.Error(write2JsonFormat("error"));
                    break;
                default:
                    break;
            }
        }
      
        private class NoopDisposable : IDisposable
        {
            public static NoopDisposable Instance = new NoopDisposable();

            public void Dispose()
            {
            }
        }
    }
}
