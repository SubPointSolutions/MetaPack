using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.NuGet.Utils
{
    public class DelegateTraceListener : TraceListener
    {
        private Action<string> _write;

        public DelegateTraceListener(Action<string> write)
        {
            _write = write;
        }

        public override void Write(string message)
        {
            _write(message);
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
