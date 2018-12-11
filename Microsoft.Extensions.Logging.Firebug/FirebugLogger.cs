using System;

namespace Microsoft.Extensions.Logging.Firebug
{
    public class FirebugLogger : ILogger
    {
        private readonly FirebugConfig _firebugConfig;
        private readonly string _category;
        private Action<LogLevel, string> _fireAction;

        public FirebugLogger(string name, FirebugConfig config)
        {
            _category = name;
            _firebugConfig = config;
            _fireAction = config.FireAction;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return NoopDisposable.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _firebugConfig.Fitler == null ? true : _firebugConfig.Fitler.Invoke(_category, logLevel);
        }

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

            string message = formatter(state, exception);
            if (!Counter.Instance.WaitOne())
            {
                return;
            }

            try
            {
                _fireAction?.Invoke(logLevel, message);
            }
            catch (Exception)
            {
                //eat it .
            }
        }
    }
}
