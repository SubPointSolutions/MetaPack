using System;

namespace MetaPack.Core.Services.Trace
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
