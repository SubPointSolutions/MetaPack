using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Services
{
    public class TraceEventEventArgs
    {
        #region properties

        public string Message { get; set; }
        public TraceEventLevel Level { get; set; }

        #endregion
    }
    public enum TraceEventLevel
    {
        Unknown,

        Critical,
        Error,
        Warning,
        Information,
        Verbose
    }
}
