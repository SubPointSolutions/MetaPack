using System.Diagnostics;
using MetaPack.Core;
using MetaPack.Core.Services.Base;

namespace MetaPack.NuGet.Utils
{
    internal class LogUtils
    {
        public static void Log(string message)
        {
            var logService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();

            logService.Information(0, message);
        }
    }
}
