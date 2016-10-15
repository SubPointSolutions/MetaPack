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

        [Option("url", HelpText = "SharePoint web site URL")]
        public string Url { get; set; }

        [Option("username", HelpText = "SharePoint user name")]
        public string UserName { get; set; }

        [Option("userpassword", HelpText = "SharePoint user password")]
        public string UserPassword { get; set; }

        [Option("spversion", HelpText = "SharePoint version with. 13/16/o365")]
        public string SharePointVersion { get; set; }
    }
}
