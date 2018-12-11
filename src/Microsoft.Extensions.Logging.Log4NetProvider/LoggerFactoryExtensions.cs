using Microsoft.Extensions.Logging;
using System;

namespace FM
{  
    public static class LoggerFactoryExtensions
    {   
        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configPath)
        {
            return AddLog4Net(factory, LogLevel.Debug, configPath);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, LogLevel minLevel, string configPath)
        {
            return AddLog4Net(
               factory,
               (_, logLevel) => logLevel >= minLevel,
               configPath);
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, Func<string, LogLevel, bool> filter, string configPath)
        {
            factory.AddProvider(new Log4NetLoggerProvider(filter, configPath));
            return factory;
        }
    }
}