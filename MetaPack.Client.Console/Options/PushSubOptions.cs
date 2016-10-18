using CommandLine;
using MetaPack.Client.Console.Options.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options
{
    public class PushSubOptions : MetaPackSubOptionsBase
    {
        #region properties

        [Option("package", HelpText = "Name of the target package file", Required = true)]
        public string Package { get; set; }

        [Option("source", HelpText = "Specifies the NuGet gallery server URL")]
        public string Source { get; set; }

        [Option("apikey", HelpText = "The API key for the target repository", Required = true)]
        public string ApiKey { get; set; }

        #endregion
    }
}
