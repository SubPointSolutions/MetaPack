using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options
{

    public class DefaultOptions
    {
        #region constructors
        public DefaultOptions()
        {
        }

        #endregion

        #region properties

        [VerbOption("install", HelpText = "Install target package")]
        public InstallSubOptions Install { get; set; }


        [VerbOption("update", HelpText = "Update target package")]
        public UpdateSubOptions Update { get; set; }


        [VerbOption("list", HelpText = "List available packages")]
        public ListSubOptions List { get; set; }

        #endregion
    }
}
