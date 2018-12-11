namespace Microsoft.Extensions.Logging.Firebug
{
    public static class LoggerFactoryExtensions
    {
        public static void AddFirebug(this LoggerFactory loggerFactory, FirebugConfig config)
        {
            loggerFactory.AddProvider(new FirebugLoggerProvider(config));
        }
    }
}
