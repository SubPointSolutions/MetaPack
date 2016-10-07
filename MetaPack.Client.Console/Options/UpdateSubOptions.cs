using CommandLine;
using MetaPack.Client.Console.Options.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options
{
    public class UpdateSubOptions : MetaPackSubOptionsBase
    {
        [Option("version", HelpText = "Version of the package.")]
        public string Version { get; set; }
    }
}
