using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Common.Services;
using MetaPack.Core.Services;

namespace MetaPack.Client.Desktop.Impl.Services
{
    public class MetaPackUITraceService : MetaPack.Client.Common.Services.MetaPackClientTraceService
    {
        public MetaPackUITraceService()
        {
            this.IsVerboseEnabled = true;
        }

        protected override void InternalWrite(TraceEventLevel level, string message)
        {
            base.InternalWrite(level, message);

            if (OnTrace != null)
            {
                OnTrace(this, new TraceServiceEventArgs
                {
                    Message = message
                });
            }
        }

        public event EventHandler<TraceServiceEventArgs> OnTrace;
    }

    public class TraceServiceEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

}
