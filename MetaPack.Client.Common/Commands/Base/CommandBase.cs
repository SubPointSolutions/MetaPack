using MetaPack.Core;
using MetaPack.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Services.Trace;

namespace MetaPack.Client.Common.Commands.Base
{
    public abstract class CommandBase
    {
        #region static
        static CommandBase()
        {
            DefaultOut = Console.Out;
        }

        #endregion

        #region constructros

        protected CommandBase()
        {
            Out = DefaultOut;

            PackageSources = new List<string>();

            SharePointVersion = "O365";
            SharePointEdition = "Standard";
            SharePointApi = "CSOM";
        }

        #endregion

        #region static

        public static TextWriter DefaultOut { get; set; }
        protected virtual EventableTraceServiceBase EventableTraceService
        {
            get
            {
                return MetaPackServiceContainer.Instance
                                               .GetService<TraceServiceBase>() as EventableTraceServiceBase;
            }
        }


        public event EventHandler<TraceEventEventArgs> OnTraceEvent;

        #endregion

        #region properties

        public bool PreRelease { get; set; }

        public abstract string Name { get; set; }

        public string Url { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public List<string> PackageSources { get; set; }

        public string ToolId { get; set; }
        public string ToolVersion { get; set; }

        public string SharePointApi { get; set; }
        public string SharePointVersion { get; set; }
        public string SharePointEdition { get; set; }

        public TextWriter Out { get; set; }

        #endregion

        #region methods

        protected virtual bool IsSharePointOnline
        {
            get
            {
                return SharePointVersion.ToUpper() == "O365";
            }
        }

        public abstract object Execute();

        protected virtual void WithEmitingTraceEvents(Action action)
        {
            try
            {
                if (EventableTraceService != null)
                    EventableTraceService.OnTraceEvent += CommandBase_OnTraceEvent;

                action();
            }
            finally
            {
                if (EventableTraceService != null)
                    EventableTraceService.OnTraceEvent -= CommandBase_OnTraceEvent;
            }
        }

        private void CommandBase_OnTraceEvent(object sender, TraceEventEventArgs e)
        {
            InvokeOnTraceEvent(sender, e);
        }

        private void InvokeOnTraceEvent(object sender, TraceEventEventArgs e)
        {
            if (OnTraceEvent != null)
                OnTraceEvent(sender, e);
        }

        #endregion

    }
}
