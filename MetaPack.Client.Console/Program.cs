using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Console.Options;
using CommandLine;
using MetaPack.Client.Common.Commands;
using MetaPack.Client.Console.Consts;
using System.Reflection;
using MetaPack.Client.Common.Services;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetaPack.Core.Utils;
using MetaPack.NuGet.Services;
using MetaPack.Client.Console.Options.Base;

namespace MetaPack.Client.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new MetaPackConsoleClient();
            var result = client.Run(args);

            Environment.Exit(result);
        }
    }
}
