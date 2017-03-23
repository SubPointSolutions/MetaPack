using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using MetaPack.Client.Common.Commands;
using MetaPack.Client.Common.Services;
using MetaPack.Client.Console.Consts;
using MetaPack.Client.Console.Options;
using MetaPack.Client.Console.Options.Base;
using MetaPack.Core;
using MetaPack.Core.Services;
using MetaPack.Core.Utils;
using MetaPack.NuGet.Services;

namespace MetaPack.Client.Console
{
    public class MetaPackConsoleClient
    {
        #region constructors

        public MetaPackConsoleClient()
        {
            Args = new string[0];

            ClientTraceService = new MetaPackClientTraceService();
            CmdParser = new Parser(settings =>
            {
                settings.IgnoreUnknownArguments = true;
            });
        }

        #endregion

        #region fields

        private bool hadFirstRun;

        #endregion

        #region properties

        protected Parser CmdParser { get; set; }

        protected string WorkingDirectory { get; set; }

        public static MetaPackClientTraceService ClientTraceService { get; private set; }


        protected virtual bool HasArgsKey(string key)
        {
            return Args.Any(s => !string.IsNullOrEmpty(s) &&
                                     s.ToLower() == "--" + key || s.ToLower() == key);
        }

        protected virtual bool IsQuiet
        {
            get
            {
                return HasArgsKey("quiet");
            }
        }

        protected virtual bool IsHelp
        {
            get
            {
                return HasArgsKey("help");
            }
        }

        public string[] Args { get; private set; }
        public string LogFile { get; private set; }

        #endregion

        #region methods



        public int Run(string[] args)
        {
            Args = args;

            if (!args.Any())
                args = new string[1] { "help" };

            var result = 0;
            var options = new DefaultOptions();

            // get the log file location
            var preDefaultArgs = new EmptySubOptionsBase();
            CmdParser.ParseArguments(args, preDefaultArgs);

            if (!string.IsNullOrEmpty(preDefaultArgs.LogFile))
                LogFile = preDefaultArgs.LogFile;

            if (!CmdParser.ParseArguments(args, options, (verb, subOption) =>
            {
                if (!hadFirstRun)
                {
                    ConfigureServiceContainer();
                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

                    hadFirstRun = true;
                }

                var currentFilePath = GetType().Assembly.Location;
                var currentFolderPath = new DirectoryInfo(currentFilePath).Parent.FullName;

                Info(string.Format("Metapack client v{0}", GetCurrentClientVersion()));
                Info(string.Format("Working directory: [{0}]", currentFolderPath));

                WorkingDirectory = currentFolderPath;
                ParseAppConfig();

                if (options.List != null)
                    result = HandleListCommand(args, options);
                else if (options.Install != null)
                    result = HandleInstallCommand(args, options);
                else if (options.Update != null)
                    result = HandleUpdateCommand(args, options);
                else if (options.Push != null)
                    result = HandlePushCommand(args, options);
                else if (options.Version != null)
                    result = HandleVersionCommand(args, options);
                else if (IsHelp)
                {
                    Info(options.GetUsage());
                    result = 0;
                }
                else
                    result = HandleMissedCommand(options);
            }))
            {
                if (!IsHelp)
                    HandleWrongArgumentParsing();
            }

            return result;
        }

        protected virtual string GetCurrentClientVersion()
        {
            var currentFilePath = GetType().Assembly.Location;
            return FileVersionInfo.GetVersionInfo(currentFilePath).FileVersion;
        }

        protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Info(string.Format("Requested assembly: " + args.Name));
            Info(string.Format("Requested assembly by: " + args.RequestingAssembly));

            if (args.Name.ToLower().Contains(".resources,"))
                return null;

            return LoadAssembliesFromDepsDirectory(sender, args);
        }

        protected virtual void ConfigureServiceContainer()
        {
            var instance = MetaPackServiceContainer.Instance;

            if (IsQuiet)
            {
                ClientTraceService.IsConsoleWriterEnabled = false;
                ClientTraceService.IsDebugWriterEnabled = false;
                ClientTraceService.IsTraceWriterEnabled = false;
            }

            if (!string.IsNullOrEmpty(LogFile))
            {
                if (Path.IsPathRooted(LogFile))
                    Directory.CreateDirectory(Path.GetDirectoryName(LogFile));

                ClientTraceService.LogFilePath = LogFile;
            }

            instance.ReplaceService(typeof(TraceServiceBase), ClientTraceService);
        }

        protected virtual Assembly LoadAssembliesFromDepsDirectory(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";

            var folderPath = Path.GetDirectoryName(WorkingDirectory);
            var so2013CSOMPath = Path.Combine(folderPath, "sp2013-csom");

            var assemblyPath = Path.Combine(so2013CSOMPath, assemblyName);

            Info(string.Format("Metapack console client - Loading assembly [{0}] from [{1}]", assemblyName, assemblyPath));

            if (!File.Exists(assemblyPath)) return null;

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }

        protected virtual void ParseAppConfig()
        {
            try
            {
                var toolResolutionService = MetaPackServiceContainer.Instance.GetService<ToolResolutionService>();

                if (toolResolutionService == null)
                    toolResolutionService = new ToolResolutionService();

                toolResolutionService.PackageSources.Clear();

                // environment varioable must go first so that we can override stuff
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.Machine);
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.User);
                toolResolutionService.InitPackageSourcesFromGetEnvironmentVariable("MetaPack.NuGet.Galleries", EnvironmentVariableTarget.Process);

                toolResolutionService.InitPackageSourcesFromString(ConfigurationManager.AppSettings["NuGet.Galleries"]);

                toolResolutionService.RefreshPackageManager();

                DefaultValues.DefaultNuGetRepositories.AddRange(toolResolutionService.PackageSources);

                MetaPackServiceContainer.Instance.ReplaceService(typeof(ToolResolutionService), toolResolutionService);

                Info(string.Format("Using NuGet galleries:[{0}]",
                      Environment.NewLine + Environment.NewLine + "    " + string.Join(Environment.NewLine + "    ", DefaultValues.DefaultNuGetRepositories) + Environment.NewLine));

            }
            catch (Exception e)
            {
                Info(string.Format("There was an error reading app.config: [{0}]", e));
                Info(string.Format("Using default NuGet gallery:[{0}]", DefaultValues.DefaultNuGetRepositories));
            }
        }

        protected virtual void HandleWrongArgumentParsing()
        {
            Info("Cannot find valid arguments. Review command line arguments and run again.");
            //Environment.Exit(CmdParserExitCodeFail);
        }

        protected virtual int HandleMissedCommand(DefaultOptions options)
        {
            Info(options.GetUsage());

            return 1;
        }

        protected virtual int HandlePushCommand(string[] args, DefaultOptions options)
        {
            var op = options.Push;

            ConfigureServices(op);

            if (!CmdParser.ParseArguments(args, op))
                return 1;

            Info(string.Format("Resolving package path [{0}]", op.Package));
            var packageFileFullPath = Path.GetFullPath(op.Package);
            Info(string.Format("Resolved package path [{0}] into [{1}]", op.Package, packageFileFullPath));

            if (!File.Exists(packageFileFullPath))
            {
                Info(string.Format("File does not exist:[{0}]", packageFileFullPath));
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

            return 0;
        }

        protected virtual int HandleUpdateCommand(string[] args, DefaultOptions options)
        {
            var op = options.Update;

            ConfigureServices(op);

            if (!CmdParser.ParseArguments(args, op))
                return 1;

            var command = new DefaultUpdateCommand
            {
                Source = op.Source,
                Url = op.Url,
                Id = op.Id,
                Version = op.Version,
                PreRelease = op.PreRelease,
                UserName = op.UserName,
                UserPassword = op.UserPassword,

                SharePointVersion = op.SharePointVersion,
                SharePointEdition = op.SharePointEdition,
                SharePointApi = op.SharePointApi,

                Force = op.Force
            };

            if (string.IsNullOrEmpty(op.SharePointVersion))
                command.SharePointVersion = SharePointRuntimVersions.O365;

            command.Execute();

            return 0;
        }

        protected virtual int HandleInstallCommand(string[] args, DefaultOptions options)
        {
            var op = options.Install;

            ConfigureServices(op);

            if (!CmdParser.ParseArguments(args, op))
                return 1;

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
                UserPassword = op.UserPassword,

                SharePointVersion = op.SharePointVersion,
                SharePointEdition = op.SharePointEdition,
                SharePointApi = op.SharePointApi,

                ToolId = op.ToolId,
                ToolVersion = op.ToolVersion,

                Force = op.Force
            };

            if (string.IsNullOrEmpty(op.SharePointVersion))
                command.SharePointVersion = SharePointRuntimVersions.O365;

            command.Execute();

            return 0;
        }

        protected virtual int HandleListCommand(string[] args, DefaultOptions options)
        {
            var op = options.List;

            ConfigureServices(op);

            if (!CmdParser.ParseArguments(args, op))
                return 1;

            var command = new NuGetListCommand
            {
                Url = op.Url,
                UserName = op.UserName,
                UserPassword = op.UserPassword
            };

            if (string.IsNullOrEmpty(op.SharePointVersion))
                command.SharePointVersion = SharePointRuntimVersions.O365;

            command.Execute();

            return 0;
        }

        private int HandleVersionCommand(string[] args, DefaultOptions options)
        {


            return 0;
        }

        protected virtual void ConfigureServices(MetaPackSubOptionsBase option)
        {
            ClientTraceService.IsVerboseEnabled = option.Verbose;
        }

        protected virtual void Info(string msg)
        {
            MetaPackTrace.Info(msg);
        }

        #endregion
    }
}
