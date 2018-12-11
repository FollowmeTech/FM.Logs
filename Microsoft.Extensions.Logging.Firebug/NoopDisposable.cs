using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging.Firebug
{
    class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance = new NoopDisposable();

        public void Dispose()
        {
        }
    }
}
