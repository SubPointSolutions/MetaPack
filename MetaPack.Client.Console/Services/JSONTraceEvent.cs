using MetaPack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Services
{
    public class JSONTraceEvent
    {
        #region properties
        public string Message { get; set; }
        public TraceEventLevel Level { get; set; }
        public DateTime TimeStamp { get; set; }

        #endregion
    }
}
