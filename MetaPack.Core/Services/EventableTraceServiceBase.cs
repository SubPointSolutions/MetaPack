using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Services
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
