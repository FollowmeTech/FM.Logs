using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace FM
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly Func<string, LogLevel, bool> _filter;
        private readonly ILoggerRepository _loggerRepository;

        public Log4NetLoggerProvider(Func<string, LogLevel, bool> filter, string configPath)
        {
            _filter = filter;
            if(_loggerRepository == null)
            {
                var name = Guid.NewGuid().ToString();
                LogManager.CreateRepository(name);
                _loggerRepository = LogManager.GetRepository(name);
                XmlConfigurator.Configure(_loggerRepository, new FileInfo(configPath));
            }
        }

        /// <inheritdoc /> 
        public ILogger CreateLogger(string categoryName)
        {
            return new Log4NetLogger(categoryName, _filter, _loggerRepository);
        }

        public void Dispose()
        {
        }
    }
}
