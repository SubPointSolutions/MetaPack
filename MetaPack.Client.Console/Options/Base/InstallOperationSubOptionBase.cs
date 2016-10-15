using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options.Base
{
    public class InstallOperationSubOptionBase : SharePointSubOptionsBase
    {
        #region properties

        [Option("id", HelpText = "ID of the target package.", Required = true)]
        public string Id { get; set; }

        [Option("version", HelpText = "Version of the package.")]
        public string Version { get; set; }

        [Option("source", HelpText = "Source of the NuGet gallery. Default is 'http://metapackgallery.com/api/v2'")]
        public string Source { get; set; }

        #endregion
    }
}
