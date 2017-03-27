using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Services
{
    public abstract class TraceEventFormatServiceBase
    {
        public abstract string FormatEvent(TraceEventFormatOptions options);
    }

    public class TraceEventFormatOptions
    {
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
    }
}
