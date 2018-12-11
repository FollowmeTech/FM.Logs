using System;
using System.Threading;

namespace Microsoft.Extensions.Logging.Firebug
{
    internal class Counter
    {
        public static Counter Instance { get; private set; }

        public static void Set(int fretimes)
        {
            if (Instance != null)
            {
                throw new Exception("Counter已经初始化过");
            }

            Instance = new Counter(fretimes);
        }

        private readonly Timer timer;
        private readonly int freqTimes;
        private int sendTimes;

        public Counter(int fretimes)
        {
            freqTimes = fretimes;
            timer = new Timer((obj) =>
            {
                Interlocked.Exchange(ref sendTimes, 0);
            }, null, 0, 1000 * freqTimes);
        }

        public bool WaitOne()
        {
            if (sendTimes >= freqTimes)
            {
                return false;
            }

            Interlocked.Increment(ref sendTimes);
            return true;
        }

        public void Release()
        {
            Interlocked.Decrement(ref sendTimes);
        }
    }
}
