using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.Data;
using MetaPack.Client.Desktop.Impl.Events;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Services;
using SubPointSolutions.Shelly.Desktop.Events.App;
using MetaPack.Client.Desktop.Impl.ViewModels;
using System.Collections.Generic;
using System.Linq;
using MetaPack.Client.Common.Commands;
using MetaPack.NuGet.Services;
using NuGet;
using MetaPack.Client.Desktop.Impl.Commands;

namespace MetaPack.Client.Desktop.Impl.Services
{
    public class MetaPackDataService : ShAppDataServiceBase
    {
        public MetaPackDataService()
        {
            CurrentSolution = new MetaPackSolution();
            CurrentOptions = new MetaPackOptions();

            ReceiveEvent<ShOnAppLoadCompletedEvent>(OnAppLoadCompleted);
            ReceiveEvent<ShOnAppExitEvent>(OnAppExitEvent);

            NuGetConnections = new BindingList<NuGetGalleryConnectionViewModel>();
            SharePointConnections = new BindingList<SharePointConnectionViewModel>();

            AvailableNuGetPackages = new BindingList<NuGetPackageViewModel>();
            InstalledNuGetPackages = new BindingList<NuGetPackageViewModel>();

            Settings = new MetaPackDesktopSettings();
        }

        public BindingList<SharePointConnectionViewModel> SharePointConnections { get; set; }
        public BindingList<NuGetGalleryConnectionViewModel> NuGetConnections { get; set; }

        private void OnAppLoadCompleted(ShOnAppLoadCompletedEvent obj)
        {
            var dataSerializationService = ShServiceContainer.Instance.GetAppDataService<ShSerializationDataService>();
            var dataPersistanceService =
                ShServiceContainer.Instance.GetAppDataService<ShAppPersistentStorageDataService>();

            var appSetting = dataPersistanceService.LoadTextData("app-options.config");

            if (!string.IsNullOrEmpty(appSetting))
            {
                CurrentOptions =
                    dataSerializationService.Deserialize(typeof(MetaPackOptions), appSetting) as MetaPackOptions;
            }

            LoadNuGetConnections();
            LoadSharePointConnections();
            LoadSettings();
        }

        private void LoadSettings()
        {
            var data = LoadPersistedData<MetaPackDesktopSettings>("metapack-settings.xml");
            Settings = data;
        }

        private void SaveSettings()
        {
            PersistData(Settings, "metapack-settings.xml");
        }


        private void PersistData(object data, string persistenseId)
        {
            var serializationService = ShServiceContainer.Instance.SerializationService;
            var dataPersistanceService = ShServiceContainer.Instance.GetAppDataService<ShAppPersistentStorageDataService>();

            var xmlData = serializationService.Serialize(data);
            dataPersistanceService.SaveTextData(persistenseId, xmlData);
        }


        private void OnAppExitEvent(ShOnAppExitEvent obj)
        {
            SaveNuGetConnections();
            SaveSharePointConnections();
            SaveSettings();
        }



        private void SaveNuGetConnections()
        {
            PersistData(NuGetConnections.Select(s => s.Dto).ToList(), "nuget-connections.xml");
        }

        private void SaveSharePointConnections()
        {
            PersistData(SharePointConnections.Select(s => s.Dto).ToList(), "sharepoint-connections.xml");
        }

        private void LoadNuGetConnections()
        {
            var data = LoadPersistedData<List<NuGetGalleryConnection>>("nuget-connections.xml");
            var viewModels = NuGetGalleryConnectionViewModel.FromDto<NuGetGalleryConnectionViewModel>(data);

            NuGetConnections.AddRange(viewModels);
        }

        private void LoadSharePointConnections()
        {
            var data = LoadPersistedData<List<SharePointConnection>>("sharepoint-connections.xml");
            var viewModels = SharePointConnectionViewModel.FromDto<SharePointConnectionViewModel>(data);

            SharePointConnections.AddRange(viewModels);
        }

        private T LoadPersistedData<T>(string persistenseId)
            where T : class, new()
        {
            var serializationService = ShServiceContainer.Instance.SerializationService;
            var dataPersistanceService = ShServiceContainer.Instance.GetAppDataService<ShAppPersistentStorageDataService>();

            var data = dataPersistanceService.LoadTextData(persistenseId);
            var resultData = new T();

            if (!string.IsNullOrEmpty(data))
                resultData = serializationService.Deserialize(typeof(T), data) as T;

            return resultData;
        }

        public override void Init()
        {
        }

        public MetaPackSolution CurrentSolution { get; set; }
        public MetaPackOptions CurrentOptions { get; set; }

        private string _lastFileLocation;

        public void OpenSolution()
        {
            var serializationService = ShServiceContainer.Instance.SerializationService;
            ShSerializationDataService.RegisterKnownTypes(typeof(MetaPackSolution).Assembly.GetTypes());

            var dlg = new OpenFileDialog
            {
                Filter = "MetaPack Solution (.mpack)|*.mpack|All Files (*.*)|*.*",
                FilterIndex = 0
            };

            var result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                _lastFileLocation = dlg.FileName;

                var data = File.ReadAllText(dlg.FileName);
                var solution = serializationService.Deserialize(typeof(MetaPackSolution), data) as MetaPackSolution;

                CurrentSolution = solution;

                RaiseEvent(new SolutionEvent
                {
                    EventType = SolutionEventType.Opened,
                    Item = solution
                });
            }
        }

        public void SaveSolution()
        {
            SaveSolution(false);
        }

        public void SaveSolution(bool showSaveAsDialog)
        {
            var solution = CurrentSolution;

            if (solution != null)
            {
                var targetFile = _lastFileLocation;

                if (string.IsNullOrEmpty(targetFile) || showSaveAsDialog)
                {
                    var dlg = new SaveFileDialog
                    {
                        Filter = "MetaPack Solution (.mpack)|*.mpack|All Files (*.*)|*.*",
                        FilterIndex = 0,
                        RestoreDirectory = true
                    };

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        targetFile = dlg.FileName;
                        _lastFileLocation = targetFile;
                    }
                }

                if (!string.IsNullOrEmpty(targetFile))
                {
                    ShSerializationDataService.RegisterKnownTypes(typeof(MetaPackSolution).Assembly.GetTypes());

                    var serializationService = ShServiceContainer.Instance.SerializationService;

                    var workspaceContent = serializationService.Serialize(solution);
                    File.WriteAllText(targetFile, workspaceContent);
                }

                RaiseEvent(new SolutionEvent
                {
                    EventType = SolutionEventType.Saved,
                    Item = solution
                });
            }
        }

        public void NewSolution()
        {
            CurrentSolution = new MetaPackSolution();

            RaiseEvent(new SolutionEvent
            {
                EventType = SolutionEventType.New,
                Item = CurrentSolution
            });
        }

        public IEnumerable<NuGetPackageViewModel> FetchAvailableNuGetPackages(
            string searchPattern,
            bool incluePrerelease)
        {
            if (searchPattern == null)
                searchPattern = string.Empty;

            var connections = NuGetConnections;

            var repo = new AggregateRepository(PackageRepositoryFactory.Default, connections.Select(s => s.Url), true);
            var allPackages = repo.Search(searchPattern, new string[]
            {
            }
            , incluePrerelease);

            if (!incluePrerelease)
            {
                allPackages = allPackages
                                .Where(p => p.IsLatestVersion);
            }

            return allPackages.OrderBy(p => p.Title)
                              .Take(20)
                              .ToList()
                              .Select(p => new NuGetPackageViewModel(p));
        }

        public BindingList<NuGetPackageViewModel> AvailableNuGetPackages { get; set; }
        public BindingList<NuGetPackageViewModel> InstalledNuGetPackages { get; set; }

        public SharePointConnectionViewModel ActiveSharePointConnection { get; set; }

        public MetaPackDesktopSettings Settings { get; set; }

        internal IEnumerable<NuGetPackageViewModel> FetchInstalledNuGetPackages()
        {
            return FetchInstalledNuGetPackages(this.ActiveSharePointConnection);
        }

        internal IEnumerable<NuGetPackageViewModel> FetchInstalledNuGetPackages(SharePointConnectionViewModel connection)
        {
            return FetchInstalledNuGetPackages(connection.Dto);
        }

        internal IEnumerable<NuGetPackageViewModel> FetchInstalledNuGetPackages(SharePointConnection connection)
        {
            var result = new List<NuGetPackageViewModel>();
            var command = new NuGetListCommand();

            command.Url = connection.Url;

            if (connection.AuthMode.Id == SharePointConnectionAuthMode.SharePointOnline.Id)
            {
                command.SharePointVersion = "o365";

                command.UserName = connection.UserName;
                command.UserPassword = connection.UserPassword;
            }
            else if (connection.AuthMode.Id == SharePointConnectionAuthMode.WindowsAuthentication.Id)
            {
                command.SharePointVersion = "sp2013";

                if (!string.IsNullOrEmpty(connection.UserName)
                    && !string.IsNullOrEmpty(connection.UserPassword))
                {
                    command.UserName = connection.UserName;
                    command.UserPassword = connection.UserPassword;

                }
            }

            command.Execute();

            foreach (var p in command.Packages)
                result.Add(new NuGetPackageViewModel(p));

            return result;
        }
    }
}