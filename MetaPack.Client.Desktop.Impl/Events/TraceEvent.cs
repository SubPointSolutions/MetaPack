using System;
using System.Diagnostics;

namespace MetaPack.Client.Desktop.Impl.Events
{
    public class TraceEvent
    {
        public int Id { get; set; }
        public TraceEventType MessageType { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
