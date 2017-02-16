﻿using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options.Base
{
    public class MetaPackSubOptionsBase
    {
        #region properties

        [Option("verbose", HelpText = "Use verbose trace")]
        public bool Verbose { get; set; }

        [Option("debug", HelpText = "Use debug trace")]
        public bool Debug { get; set; }

        [Option("toolid", HelpText = "NuGet package ID of the tool to use for packing, unpacking and deployment operations")]
        public string ToolId { get; set; }

        [Option("toolversion", HelpText = "NuGet package Version of the tool to use for packing, unpacking and deployment operations")]
        public string ToolVersion { get; set; }

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
