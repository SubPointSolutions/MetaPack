using CommandLine;
using MetaPack.Client.Console.Options.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Options
{
    public class InstallSubOptions : InstallOperationSubOptionBase
    {
        #region propeties
        [Option("force", HelpText = "Force install package if package already exists")]
        public bool Force { get; set; }
        #endregion
    }
}
