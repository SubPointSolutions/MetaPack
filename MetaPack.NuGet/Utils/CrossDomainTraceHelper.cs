using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetaPack.Core.Utils;
using MetaPack.NuGet.Services;

namespace MetaPack.NuGet.Utils
{
    public class CrossDomainTraceHelper : MarshalByRefObject
    {
        private CrossDomainTraceHelper _parentDomain;

        public CrossDomainTraceHelper()
        {
        }

        public static void StartListening(AppDomain domain)
        {
            var listenerType = typeof(CrossDomainTraceHelper);

            // Create a remote instance
            var remoteHelper =
                (CrossDomainTraceHelper)domain.CreateInstanceAndUnwrap(
                    listenerType.Assembly.FullName,
                    listenerType.FullName);

            // Create a local instance
            var localHelper = new CrossDomainTraceHelper();

            // Register the local helper in the remote domain
            remoteHelper.Register(localHelper);
        }

        private void Register(CrossDomainTraceHelper parentDomain)
        {
            // Store the parent domain to pass messages to later
            _parentDomain = parentDomain;

            var logginService = new CrossAppDomainTraceService();
            MetaPackServiceContainer.Instance.ReplaceService(typeof(TraceServiceBase), logginService);

            CrossAppDomainTraceService.OnTraceEvent += (s, e) =>
            {
                Write(e.TraceString);
            };
        }

        private void Write(string message)
        {
            _parentDomain.RemoteWrite(message);
        }

        private void RemoteWrite(string message)
        {
            var loggingService = MetaPackServiceContainer.Instance.GetService<TraceServiceBase>();
            loggingService.Information(0, message);
        }
    }
}
