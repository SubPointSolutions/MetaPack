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

namespace MetaPack.Client.Console
{
    class Program
    {
        private static string WorkingDirectory { get; set; }

        static void Main(string[] args)
        {
            ConfigureServiceContainer();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var currentFilePath = typeof(Program).Assembly.Location;
            var currentFolderPath = new DirectoryInfo(currentFilePath).Parent.FullName;

            Log(string.Format("Loading metapack v{0}", FileVersionInfo.GetVersionInfo(currentFilePath).FileVersion));
            Log(string.Format("Working directory: [{0}]", currentFolderPath));

            WorkingDirectory = currentFolderPath;
            ParseAppConfig();

            if (!args.Any())
                args = new string[1] { "help" };

            var options = new DefaultOptions();

            if (!Parser.Default.ParseArguments(args, options, (verb, subOption) =>
            {
                if (options.List != null)
                {
                    HandleListCommand(args, options);
                }
                else if (options.Install != null)
                {
                    HandleInstallCommand(args, options);
                }
                else if (options.Update != null)
                {
                    HandleUpdateCommand(args, options);
                }
                else if (options.Push != null)
                {
                    HandlePushCommand(args, options);
                }
                else
                    HandleMissedCommand(options);
            }))
            {
                HandleWrongArgumentParsing();
            }

            Environment.Exit(0);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Log(string.Format("Failing while resolving assembly..."));

            Log(string.Format("Requested assembly: " + args.Name));
            Log(string.Format("Requested assembly by: " + args.RequestingAssembly));

            return null;
        }

        private static void ConfigureServiceContainer()
        {
            var instance = MetaPackServiceContainer.Instance;

            var traceService = new ConsoleTraceService();
            instance.ReplaceService(typeof(TraceServiceBase), traceService);
        }

        static Assembly LoadAssembliesFromWorkingDirectory(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";

            var folderPath = Path.GetDirectoryName(WorkingDirectory);
            var assemblyPath = Path.Combine(folderPath, assemblyName);

            Log(string.Format("Loading assenbly [{0}] from [{1}]", assemblyName, assemblyPath));

            if (!File.Exists(assemblyPath)) return null;

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
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
                        DefaultValues.DefaultNuGetRepositories.Clear();
                        DefaultValues.DefaultNuGetRepositories.AddRange(urls);

                        Log(string.Format("Using default NuGet gallery from app.config:[{0}]",
                          Environment.NewLine + string.Join(Environment.NewLine + "    ", DefaultValues.DefaultNuGetRepositories) + Environment.NewLine));
                    }
                    else
                    {
                        Log(string.Format("Using default NuGet gallery:[{0}]", DefaultValues.DefaultNuGetRepositories));
                    }

                }
                else
                {
                    Log(string.Format("'NuGet.Galleries' is null or empty. Using default NuGet gallery:[{0}]",
                        DefaultValues.DefaultNuGetRepositories));
                }
            }
            catch (Exception e)
            {
                Log(string.Format("There was an error reading app.config: [{0}]", e));
                Log(string.Format("Using default NuGet gallery:[{0}]", DefaultValues.DefaultNuGetRepositories));
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

        private static void HandlePushCommand(string[] args, DefaultOptions options)
        {
            var op = options.Push;

            if (!Parser.Default.ParseArguments(args, op))
                Environment.Exit(Parser.DefaultExitCodeFail);

            Log(string.Format("Resolving package path [{0}]", op.Package));
            var packageFileFullPath = Path.GetFullPath(op.Package);
            Log(string.Format("Resolved package path [{0}] into [{1}]", op.Package, packageFileFullPath));

            if (!File.Exists(packageFileFullPath))
            {
                Log(string.Format("File does not exist:[{0}]", packageFileFullPath));
            }

            using (var stream = File.Open(packageFileFullPath, FileMode.Open))
            {
                var command = new DefaultNuGetPushCommand
                {
                    Source = op.Source,
                    ApiKey = op.ApiKey,

                    Package = stream

                };

                command.Execute();
            }
        }

        private static void HandleUpdateCommand(string[] args, DefaultOptions options)
        {
            var op = options.Update;

            if (!Parser.Default.ParseArguments(args, op))
                Environment.Exit(Parser.DefaultExitCodeFail);

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

            var sources = new List<string>();

            if (!string.IsNullOrEmpty(op.Source))
                sources.Add(op.Source);

            sources.AddRange(DefaultValues.DefaultNuGetRepositories);

            var command = new DefaultInstallCommand
            {
                PackageSources = sources,
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
            MetaPackTrace.WriteLine(msg);
        }
    }
}
