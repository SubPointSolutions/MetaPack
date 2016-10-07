using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options.Base
{
    public class MetaPackSubOptionsBase
    {
        [Option("id", HelpText = "ID of the target package.")]
        public string Id { get; set; }

        [Option("source", HelpText = "Source of the NuGet gallery or repository.")]
        public string Source { get; set; }

        [Option("prerelease", HelpText = "Should include pre-release versions of packages.")]
        public bool PreRelease { get; set; }

        [Option("verbose", HelpText = "Use verbose trace.")]
        public bool Verbose { get; set; }

        [Option("debug", HelpText = "Use debug trace.")]
        public bool Debug { get; set; }
    }
}
