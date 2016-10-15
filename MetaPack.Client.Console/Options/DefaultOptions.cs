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

        [VerbOption("install", HelpText = "Install package to SharePoint web site. Use 'install --help' for more information.")]
        public InstallSubOptions Install { get; set; }


        [VerbOption("update", HelpText = "Update package on SharePoint web site. Use 'update --help' for more information.")]
        public UpdateSubOptions Update { get; set; }


        [VerbOption("list", HelpText = "List installed packaged on SharePoit web site. Use 'list --help' for more information.")]
        public ListSubOptions List { get; set; }

        #endregion

        #region help

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        #endregion
    }
}
