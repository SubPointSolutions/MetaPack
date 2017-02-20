using MetaPack.Core.Services;
using System;
using System.Diagnostics;
using System.IO;

namespace MetaPack.NuGet.Services
{
    public class CrossAppDomainTraceService : TraceSourceService
    {
        protected override void TraceEvent(int id, TraceEventType messageType, object message, Exception exception)
        {
            base.TraceEvent(id, messageType, message, exception);

            var traceString = GetTraceEventString(id, messageType, message, exception);

            if (OnTraceEvent != null)
                OnTraceEvent(this, new TraceStringEventAgs
                {
                    TraceString = traceString
                });

        }

        public static EventHandler<TraceStringEventAgs> OnTraceEvent;
    }

    public class TraceStringEventAgs : EventArgs
    {
        public string TraceString { get; set; }
    }
}
