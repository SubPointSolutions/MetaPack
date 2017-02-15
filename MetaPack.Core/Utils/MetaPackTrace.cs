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
            var traceService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();
            traceService.Information(0, message);
        }

        #endregion
    }
}
