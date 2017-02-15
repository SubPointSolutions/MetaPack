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

        #region methods

        public static void WriteLine(string message)
        {
            WriteLine(message, null);
        }

        public static void WriteLine(string message, params object[] p)
        {
            var traceService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();

            if (p != null)
                traceService.Information(0, string.Format(message, p));
            else
                traceService.Information(0, message);
        }

        #endregion
    }
}
