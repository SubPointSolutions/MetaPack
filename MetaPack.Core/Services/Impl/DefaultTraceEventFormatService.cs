using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Services.Trace;

namespace MetaPack.Core.Services.Impl
{
    public class DefaultTraceEventFormatService : TraceEventFormatServiceBase
    {
        public override string FormatEvent(TraceEventFormatOptions options)
        {
            if (options == null)
                return string.Empty;

            return string.Format("[{0}] {1}", options.Level, options.Message);
        }
    }
}
