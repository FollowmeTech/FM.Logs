using System;

namespace Microsoft.Extensions.Logging.Firebug
{
    public class FirebugConfig
    {
        /// <summary>
        /// 每分钟可以firebug的数量
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// log过滤器
        /// </summary>
        public Func<string, LogLevel, bool> Fitler { get; set; }

        /// <summary>
        /// Fire log action
        /// </summary>
        public Action<LogLevel, string> FireAction { get; set; }
    }
}
