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

namespace MetaPack.Client.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentFilePath = typeof(Program).Assembly.Location;
            var currentFolderPath = new DirectoryInfo(currentFilePath).FullName;

            Log(string.Format("Loading metapack v{0}", FileVersionInfo.GetVersionInfo(currentFilePath).FileVersion));
            Log(string.Format("Working directory: [{0}]", currentFolderPath));

            ParseAppConfig();

            if (!args.Any())
                args = new string[1] { "help" };

            var options = new DefaultOptions();

            if (!Parser.Default.ParseArguments(args, options, (verb, subOption) =>
            {
                if (options.List != null)
                    HandleListCommand(args, options);
                else if (options.Install != null)
                    HandleInstallCommand(args, options);
                else if (options.Update != null)
                    HandleUpdateCommand(args, options);
                else
                    HandleMissedCommand(options);
            }))
            {
                HandleWrongArgumentParsing();
            }

            Environment.Exit(0);
        }

        private static void ParseAppConfig()
        {
            try
            {
                Log("Reading app.config 'NuGet.Galleries' value...");

                var nugetUrls = ConfigurationManager.AppSettings["NuGet.Galleries"];

                if (!string.IsNullOrEmpty(nugetUrls))
                {
                    Log(string.Format("NuGet.Galleries: [{0}]", nugetUrls));


                    var urls = nugetUrls.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                    if (urls.Any())
                    {
                        DefaultValues.DefaultNuGetRepository = urls.FirstOrDefault().Trim();

                        Log(string.Format("Using default NuGet gallery from app.config:[{0}]",
                            DefaultValues.DefaultNuGetRepository));
                    }
                    else
                    {
                        Log(string.Format("Using default NuGet gallery:[{0}]", DefaultValues.DefaultNuGetRepository));
                    }

                }
                else
                {
                    Log(string.Format("'NuGet.Galleries' is null or empty. Using default NuGet gallery:[{0}]",
                        DefaultValues.DefaultNuGetRepository));
                }
            }
            catch (Exception e)
            {
                Log(string.Format("There was an error reading app.config: [{0}]", e));
                Log(string.Format("Using default NuGet gallery:[{0}]", DefaultValues.DefaultNuGetRepository));
            }
        }

        private static void HandleWrongArgumentParsing()
        {
            Log("Cannot find arguments. Exiting.");
            Environment.Exit(Parser.DefaultExitCodeFail);
        }

        private static void HandleMissedCommand(DefaultOptions options)
        {
            Log(options.GetUsage());
            Environment.Exit(Parser.DefaultExitCodeFail);
        }

        private static void HandleUpdateCommand(string[] args, DefaultOptions options)
        {
            var op = options.Update;

            if (!Parser.Default.ParseArguments(args, op))
                Environment.Exit(Parser.DefaultExitCodeFail);

            if (string.IsNullOrWhiteSpace(op.Source))
                op.Source = DefaultValues.DefaultNuGetRepository;

            var command = new DefaultUpdateCommand
            {
                Source = op.Source,
                Url = op.Url,
                Id = op.Id,
                Version = op.Version,
                PreRelease = op.PreRelease,
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

        private static void HandleInstallCommand(string[] args, DefaultOptions options)
        {
            var op = options.Install;

            if (!Parser.Default.ParseArguments(args, op))
                Environment.Exit(Parser.DefaultExitCodeFail);

            if (string.IsNullOrWhiteSpace(op.Source))
                op.Source = DefaultValues.DefaultNuGetRepository;

            var command = new DefaultInstallCommand
            {
                Source = op.Source,
                Url = op.Url,
                Id = op.Id,
                Version = op.Version,
                PreRelease = op.PreRelease,
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

        private static void HandleListCommand(string[] args, DefaultOptions options)
        {
            var op = options.List;

            if (!Parser.Default.ParseArguments(args, op))
                Environment.Exit(Parser.DefaultExitCodeFail);

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

        public static void Log(string msg)
        {
            System.Console.WriteLine(msg);
        }
    }
}
