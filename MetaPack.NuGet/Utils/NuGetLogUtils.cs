using System.Diagnostics;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetaPack.Core.Services.Trace;

namespace MetaPack.NuGet.Utils
{
    internal class NuGetLogUtils
    {
        public static void Info(string message)
        {
            var logService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();

            logService.Information(0, message);
        }

        public static void Verbose(string message)
        {
            var logService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();

            logService.Verbose(0, message);
        }
    }
}
