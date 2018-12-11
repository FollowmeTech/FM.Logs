namespace Microsoft.Extensions.Logging.Firebug
{
    internal class FirebugLoggerProvider : ILoggerProvider
    {
        public FirebugConfig _config { get; set; }

        public FirebugLoggerProvider(FirebugConfig config)
        {
            Counter.Set(config.Frequency);
            _config = config;
        }

        /// <inheritdoc /> 
        public ILogger CreateLogger(string categoryName)
        {
            return new FirebugLogger(categoryName, _config);
        }

        public void Dispose()
        {

        }
    }
}
