using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Common.Commands.Base
{
    public abstract class CommandBase
    {
        #region constructros

        protected CommandBase()
        {
            PackageSources = new List<string>();

            SharePointVersion = "O365";
            SharePointEdition = "Standard";
            SharePointApi = "CSOM";
        }

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

        #endregion

    }
}
