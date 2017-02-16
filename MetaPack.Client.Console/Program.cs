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

            return LoadAssembliesFromDepsDirectory(sender, args);
        }

        private static void ConfigureServiceContainer()
        {
            var instance = MetaPackServiceContainer.Instance;

            var traceService = new ConsoleTraceService();
            instance.ReplaceService(typeof(TraceServiceBase), traceService);
        }

        static Assembly LoadAssembliesFromDepsDirectory(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";

            var folderPath = Path.GetDirectoryName(WorkingDirectory);
            var so2013CSOMPath = Path.Combine(folderPath, "sp2013-csom");

            var assemblyPath = Path.Combine(so2013CSOMPath, assemblyName);

            Log(string.Format("MetaPack.Client.Console - Loading assenbly [{0}] from [{1}]", assemblyName, assemblyPath));

            if (!File.Exists(assemblyPath)) return null;

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        private static List<string> ResolveNuGetGalleryPaths(string value)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(value))
            {
                var urls = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var url in urls)
                {
                    if (url.ToLower().StartsWith("http"))
                    {
                        result.Add(url);
                    }
                    else
                    {
                        var localPath = Path.GetFullPath(url);
                        result.Add(localPath);
                    }
                }
            }

            return result;
        }

        private static void ParseAppConfig()
        {
            try
            {
                Log("Reading app.config 'NuGet.Galleries' value...");


                var nugetUrls = new List<string>();

                nugetUrls.AddRange(ResolveNuGetGalleryPaths(ConfigurationManager.AppSettings["NuGet.Galleries"]));

                nugetUrls.AddRange(ResolveNuGetGalleryPaths(Environment.GetEnvironmentVariable(
                                                                        "MetaPack.NuGet.Galleries",
                                                                        EnvironmentVariableTarget.Machine)));

                nugetUrls.AddRange(ResolveNuGetGalleryPaths(Environment.GetEnvironmentVariable(
                                                                        "MetaPack.NuGet.Galleries",
                                                                        EnvironmentVariableTarget.User)));

                nugetUrls.AddRange(ResolveNuGetGalleryPaths(Environment.GetEnvironmentVariable(
                                                                        "MetaPack.NuGet.Galleries",
                                                                        EnvironmentVariableTarget.Process)));
                // updating the defaults
                DefaultValues.DefaultNuGetRepositories.Clear();
                DefaultValues.DefaultNuGetRepositories.AddRange(nugetUrls);

                Log(string.Format("Using NuGet galleries:[{0}]",
                      Environment.NewLine + Environment.NewLine + "    "+ string.Join(Environment.NewLine + "    ", DefaultValues.DefaultNuGetRepositories) + Environment.NewLine));

                var toolResolutionService = MetaPackServiceContainer.Instance.GetService<ToolResolutionService>();

                if (toolResolutionService == null)
                    toolResolutionService = new ToolResolutionService();

                toolResolutionService.PackageSources.Clear();
                toolResolutionService.PackageSources.AddRange(DefaultValues.DefaultNuGetRepositories);

                toolResolutionService.RefreshPackageManager();

                MetaPackServiceContainer.Instance.ReplaceService(typeof(ToolResolutionService), toolResolutionService);
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
            MetaPackTrace.Info(msg);
        }
    }
}
