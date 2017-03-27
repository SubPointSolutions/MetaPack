using MetaPack.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Utils
{
    public static class MetaPackTrace
    {
        #region static

        static MetaPackTrace()
        {

        }

        #endregion

        #region properties

        public static TraceServiceBase TraceService
        {
            get
            {
                return MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();
            }
        }

        #endregion

        #region methods

        public static void Error(string message)
        {
            TraceService.Error(0, message);
        }

        public static void Info(string message)
        {
            Info(message, null);
        }

        public static void Info(string message, params object[] p)
        {
            if (p != null)
                TraceService.Information(0, string.Format(message, p));
            else
                TraceService.Information(0, message);
        }

        public static void Verbose(string message)
        {
            Verbose(message, null);
        }

        public static void Verbose(string message, params object[] p)
        {
            if (p != null)
                TraceService.VerboseFormat(0, message, p);
            else
                TraceService.Verbose(0, message);
        }

        #endregion
    }
}
