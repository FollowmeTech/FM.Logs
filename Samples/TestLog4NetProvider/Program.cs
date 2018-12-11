using FM;
using System;


namespace TestLog4NetProvider
{
    using log4net;
    using log4net.Config;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Firebug;
    using System.IO;
    using System.Threading;

    internal class Program
    {
        private static void Main(string[] args)
        {
            SendWithLoggerLevel();
            //SendWithRawLog4net();
        }

        private static void SendWithLoggerLevel()
        {
            LoggerFactory loggerFactory = new LoggerFactory();
            string log4netPath = System.IO.Path.Combine(AppContext.BaseDirectory, "log4net.config");

            loggerFactory.AddLog4Net((loggername, level) =>
            {
                if (loggername == "hello")
                {
                    return false;
                }

                return true;
            }, log4netPath);

            loggerFactory.AddFirebug(new FirebugConfig
            {
                Frequency = 2,
                Fitler = (c, l) =>
                {
                    if (l == LogLevel.Error || l == LogLevel.Critical)
                    {
                        return true;
                    }

                    if (c == "hello")
                    {
                        return true;
                    }

                    return false;
                },
                FireAction = (l, c) =>
                {
                    Console.WriteLine($"[{l}]   {c}");
                }
            });


            //normal
            //ilogger ...
            ILogger logger = loggerFactory.CreateLogger(typeof(Program));
            logger.LogInformation("informat leve");
            logger.LogDebug("debug level");
            logger.LogError("Error level");

            for (int i = 0; i < 10000; i++)
            {
                Thread.Sleep(100);
                logger.LogError("error for testing:" + i);
            }

            try
            {
                throw new Exception("an exception error");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            //--access.
            ILogger accesslog = loggerFactory.CreateLogger("grpc.access");
            accesslog.LogDebug("{grpc access log}");

            //call
            ILogger calllog = loggerFactory.CreateLogger("grpc.call");
            calllog.LogDebug("grpc access log");

            ILogger helloLogger = loggerFactory.CreateLogger("hello");
            helloLogger.LogInformation("info mation from hellologger");
        }

        private static void SendWithRawLog4net()
        {
            string name = Guid.NewGuid().ToString();
            LogManager.CreateRepository(name);
            log4net.Repository.ILoggerRepository _loggerRepository = LogManager.GetRepository(name);
            XmlConfigurator.Configure(_loggerRepository, new FileInfo("log4net.config"));

            ILog abc = log4net.LogManager.GetLogger(_loggerRepository.Name, "abc");
            abc.Error("这是一个错误");

            ILog access = log4net.LogManager.GetLogger(_loggerRepository.Name, "grpc.access");
            access.Warn("access..pls pass");
        }
    }
}