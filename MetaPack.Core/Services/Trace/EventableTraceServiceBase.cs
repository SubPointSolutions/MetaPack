using System;

namespace MetaPack.Core.Services.Trace
{
    public abstract class EventableTraceServiceBase : TraceServiceBase
    {
        #region events

        public event EventHandler<TraceEventEventArgs> OnTraceEvent;

        #endregion

        #region methods

        protected virtual void InvokeOnTraceMessage(string message, TraceEventLevel level)
        {
            InvokeOnTraceMessage(new TraceEventEventArgs
            {
                Message = message,
                Level = level
            });
        }

        protected virtual void InvokeOnTraceMessage(TraceEventEventArgs args)
        {
            if (OnTraceEvent != null)
            {
                OnTraceEvent(this, args);
            }
        }

        #endregion
    }
}
