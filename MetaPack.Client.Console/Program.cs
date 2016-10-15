using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Console.Options;
using CommandLine;
using MetaPack.Client.Common.Commands;

namespace MetaPack.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Log(string.Format("Loading metapack v{0}",
                 FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).FileVersion));

            var options = new DefaultOptions();

            if (!Parser.Default.ParseArguments(args, options, (verb, subOption) =>
            {
                if (options.List != null)
                {
                    var op = options.List;

                    var command = new NuGetListCommand
                    {
                        Url = op.Url,

                        UserName = op.UserName,
                        UserPassword = op.UserPassword
                    };

                    if (!string.IsNullOrEmpty(op.SharePointVersion))
                    {
                        if ("o365" == op.SharePointVersion.ToLower())
                        {
                            command.IsSharePointOnline = true;
                        }
                    }

                    command.Execute();
                }
                else if (options.Install != null)
                {
                    Log("install");
                }
                else if (options.Update != null)
                {
                    Log("upate");
                }
                else
                {
                    Log("can't find any arguments at all. sorry");
                }
            }))
            {
                Log("Can't find any arguments. Exiting.");
                Environment.Exit(Parser.DefaultExitCodeFail);
            }

            Environment.Exit(0);
        }

        public static void Log(string msg)
        {
            System.Console.WriteLine(msg);
        }
    }
}
