using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.Data;
using MetaPack.Client.Desktop.Impl.Events;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.Views;
using NuGet;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Utils;

namespace MetaPack.Client.Desktop.Impl.Controls
{
    public partial class MetaPackControl : ShUserControlBase
    {
        public MetaPackControl()
        {
            PackageCache = new Dictionary<SharePointConnection, LocalNuGetPackageCacheItem>();

            InitializeComponent();
            InitControls();
        }

        #region properties

        public ContextMenu ConnectionsContextMenu { get; set; }

        #endregion

        private void InitControls()
        {
            ConnectionsContextMenu = new ContextMenu();
            ConnectionsContextMenu.Popup += ConnectionsContextMenu_Popup;

            tvConnections.ExpandAll();

            tvConnections.ContextMenu = ConnectionsContextMenu;
            tvConnections.Click += TvConnections_Click;
            tvConnections.AfterSelect += TvConnections_AfterSelect;

            //ReceiveEvent<SolutionEvent>(OnSolutionEvent);

            bNuGetRefresh.Click += BNuGetRefresh_Click;

            lvPackages.Columns.Clear();

            NameColumn = lvPackages.Columns.Add("Name");
            AuthColumn = lvPackages.Columns.Add("Authors");
            VersionColunm = lvPackages.Columns.Add("Version");
            LatestVersionColunm = lvPackages.Columns.Add("Latest version");

            lvPackages.FullRowSelect = true;

            lvPackages.SelectedIndexChanged += LvPackages_SelectedIndexChanged;

            bInstall.Click += BInstall_Click;

            // bInstall.Enabled = false;
            bUpdate.Enabled = false;

            var allPackages = new string[]
            {
                "B2Hubs.Taxonomy.Sales",
                "B2Hubs.Taxonomy.Business",
                "B2Hubs.Taxonomy.Years",
                "B2Hubs.Taxonomy.Australia",
                "B2Hubs.Taxonomy.ISO9000",
                "B2Hubs.Taxonomy.ISO9000",
                "B2Hubs.Taxonomy.ISO45001",
                "B2Intranet.IT",
                "B2Intranet.HR",
                "B2Intranet.Portal",
                "B2Intranet.Common",
                "B2Intranet.KB",
                "B2Intranet.IT",
                "B2Intranet.RnD",
                "B2Intranet.Sales",
                "B2Intranet.Operations",
                "B2Intranet.WebParts.WeatherWebPart",
                "B2Intranet.WebParts.News",
                "B2Intranet.WebParts.NewsPro",
                "B2Intranet.DocManagement.Lite",
                "B2Intranet.DocManagement.Pro",
                "B2Intranet.JS",
                "B2Intranet.Templates",
            };

            lvPackages.View = View.Details;
            lvPackages.FullRowSelect = true;

            lvPackages.Items.Clear();

            lvPackages.Items.Clear();

            foreach (var p in allPackages)
            {
                var item = lvPackages.Items.Add(new ListViewItem
                {
                    Text = p,
                    Tag = new PackageInfo
                    {
                        //Package = package,
                        CanUpdate = false,
                        CanInstall = false
                    }
                });

                var version = "1.0.0.0";

                if (p.Contains("B2Hubs"))
                {
                    version = "1.2.0.0";
                }

                if (p.Contains("B2Intranet.WebParts"))
                {
                    version = "1.1.10.0";
                }

                if (p.Contains("B2Intranet.DocManagement"))
                {
                    version = "0.9.0.0";
                }

                if (p.Contains("B2Intranet.DocManagement"))
                {
                    version = "1.0.0.0";
                }


                if (p.Contains("B2Hubs.Taxonomy"))
                {
                    version = "2.0.0.0";
                }

                if (p.Contains("B2Hubs.Taxonomy.ISO"))
                {
                    version = "2.2.0.0";
                }


                item.SubItems.AddRange(new string[]
                {
                    "SubPoint Solutions",
                    version,
                    version
                });
            }


            Autofit();
        }

        private void BInstall_Click(object sender, EventArgs e)
        {
            //var connectionNode = tvConnections.SelectedNode;

            //if (connectionNode == null)
            //    return;

            //var connection = connectionNode.Tag is SharePointConnection
            //    ? connectionNode.Tag as SharePointConnection
            //    : connectionNode.Parent.Tag as SharePointConnection;

            //var package = lvPackages.SelectedItems.Count > 0
            //    ? (lvPackages.SelectedItems[0].Tag as PackageInfo).Package
            //    : null;
            //if (package == null)
            //    return;

            //var deploymentControl = new PackageDeploymentView();

            //deploymentControl.SetConnection(connection);
            //deploymentControl.SetPackage(package);

            //var appService = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();

            //var nuGetConnection = appService.CurrentOptions.NuGetOptions.Connections.First();

            //ShFormUtils.ShowModal(deploymentControl,
            //    form => { },
            //    control => { control.OkButton.Text = "Install"; },
            //    (s, ee) =>
            //    {
            //        ee.Cancel = true;

            //        ShThread.DoWork(() =>
            //        {
            //            var count = 10;

            //            ee.Control.TsEnableOkButton(false);
            //            ee.Control.TsEnableCancelButton(false);

            //            var service = new MetaPackM2DeploymentService();

            //            service.Deploy(
            //                connection,
            //                nuGetConnection,
            //                package,
            //                progress =>
            //                {
            //                    deploymentControl.TsOutputProgress(
            //                        progress.Message,
            //                        progress.ProgressCurrent,
            //                        progress.ProgressMax);
            //                });

            //            //for (var i = 0; i < count; i++)
            //            //{
            //            //    deploymentControl.TsOutputProgress(
            //            //        string.Format("Progress " + i),
            //            //        i, count);

            //            //    Thread.Sleep(200 * i);
            //            //}

            //            ee.Control.TsEnableOkButton(true);
            //            ee.Control.TsEnableCancelButton(true);
            //        });
            //    },
            //    (s, ee) => { });
        }

        private void LvPackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPackages.SelectedItems.Count == 0)
                return;

            //UpdateSolutionTree(null);

            tbId.Text = (lvPackages.SelectedItems[0] as ListViewItem).Text;
            tbTitle.Text = (lvPackages.SelectedItems[0] as ListViewItem).Text;
            tbAuthors.Text = "SubPoint Solutions";

            llProjectUrl.Text = "N/A";
            llProjectUrl.Enabled = false;

            tbDescription.Text =
                "A package with common taxonomy for Australian. Includes states, cities and suburbs. Delivers 'Australia' term group and terms, related term sets and terms.";


            var selectedPackageInfo = lvPackages.SelectedItems[0].Tag as PackageInfo;

            if (selectedPackageInfo == null)
                return;

            var package = selectedPackageInfo.Package;

            if (package == null)
                return;

            tbTitle.Text = package.Title ?? string.Empty;
            tbAuthors.Text = package.Authors != null ? string.Join(", ", package.Authors) : string.Empty;
            tbId.Text = package.Id ?? string.Empty;
            tbDescription.Text = package.Description ?? string.Empty;

            if (package.ProjectUrl != null)
            {
                pbIcon.LoadAsync(package.ProjectUrl.AbsolutePath);
            }
        }

        public ColumnHeader LatestVersionColunm { get; set; }

        public ColumnHeader VersionColunm { get; set; }

        public ColumnHeader NameColumn { get; set; }

        public ColumnHeader AuthColumn { get; set; }

        private void TvConnections_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var seletedNode = tvConnections.SelectedNode;

            if (seletedNode != null)
            {
                if (seletedNode.Tag is SharePointConnection)
                {
                }
                else if (seletedNode.Tag == "Installed")
                {
                    var lookupCachedItem = LookupCachedItem(seletedNode.Parent.Tag as SharePointConnection);

                    if (lookupCachedItem == null)
                        return;

                    RefreshList(lookupCachedItem, lookupCachedItem.Installed);
                }
                else if (seletedNode.Tag == "Online")
                {
                    var lookupCachedItem = LookupCachedItem(seletedNode.Parent.Tag as SharePointConnection);

                    if (lookupCachedItem == null)
                        return;

                    RefreshList(lookupCachedItem, lookupCachedItem.Online);
                }

                bNuGetRefresh.Enabled = true;

                var connection = seletedNode.Tag as SharePointConnection;

                // RefreshNuGetPackages(connection);
            }
            else
            {
                // installed?
                //if()

                //bNuGetRefresh.Enabled = false;
            }
        }

        public class PackageInfo
        {
            public IPackage Package { get; set; }

            public bool CanInstall { get; set; }
            public bool CanUpdate { get; set; }
        }

        private void RefreshList(LocalNuGetPackageCacheItem cachedItem, List<IPackage> packages)
        {
            lvPackages.View = View.Details;
            lvPackages.FullRowSelect = true;

            lvPackages.Items.Clear();

            foreach (var package in packages.OrderBy(p => p.Id))
            {
                var latestVersion = cachedItem.Online.FirstOrDefault(p => p.Id == package.Id);

                var item = lvPackages.Items.Add(new ListViewItem
                {
                    Text = string.IsNullOrEmpty(package.Title) ? package.Id : package.Title,
                    Tag = new PackageInfo
                    {
                        Package = package,
                        CanUpdate = false,
                        CanInstall = false
                    }
                });

                item.SubItems.AddRange(new string[]
                {
                    package.Title,
                    package.Authors != null ? string.Join(", ", package.Authors) : string.Empty,
                    package.Version.ToString(),
                    latestVersion != null ? latestVersion.Version.ToString() : "unavialable"
                });
            }

            Autofit();
        }

        private bool _hasAutofit = false;

        private void Autofit()
        {
            if (!_hasAutofit && lvPackages.Items.Count > 0)
            {
                VersionColunm.Width = -2;
                NameColumn.Width = -2;
                AuthColumn.Width = -2;
                LatestVersionColunm.Width = -1;

                _hasAutofit = true;
            }
        }

        private LocalNuGetPackageCacheItem LookupCachedItem(SharePointConnection sharePointConnection)
        {
            if (sharePointConnection == null)
                return null;

            if (!PackageCache.ContainsKey(sharePointConnection))
                PackageCache.Add(sharePointConnection, new LocalNuGetPackageCacheItem());

            return PackageCache[sharePointConnection];
        }

        private void BNuGetRefresh_Click(object sender, EventArgs e)
        {
            var seletedNode = tvConnections.SelectedNode;

            if (seletedNode != null)
            {
                if (seletedNode.Tag is SharePointConnection)
                {
                }
                else if (seletedNode.Tag == "Installed")
                {
                    var lookupCachedItem = LookupCachedItem(seletedNode.Parent.Tag as SharePointConnection);
                    lookupCachedItem.Installed = FetchLocalNuGetPackages(seletedNode.Parent.Tag as SharePointConnection);

                    RefreshList(lookupCachedItem, lookupCachedItem.Installed);
                }
                else if (seletedNode.Tag == "Online")
                {
                    var lookupCachedItem = LookupCachedItem(seletedNode.Parent.Tag as SharePointConnection);

                    lookupCachedItem.Online = FetchOnlineNuGetPackages(seletedNode.Parent.Tag as SharePointConnection);
                    RefreshList(lookupCachedItem, lookupCachedItem.Online);
                }

                bNuGetRefresh.Enabled = true;

                var connection = seletedNode.Tag as SharePointConnection;

                // RefreshNuGetPackages(connection);
            }
            else
            {
                // installed?
                //if()

                //bNuGetRefresh.Enabled = false;
            }
        }

        public class LocalNuGetPackageCacheItem
        {
            public LocalNuGetPackageCacheItem()
            {
                Installed = new List<IPackage>();
                Online = new List<IPackage>();
            }

            public List<IPackage> Online { get; set; }
            public List<IPackage> Installed { get; set; }
        }

        public Dictionary<SharePointConnection, LocalNuGetPackageCacheItem> PackageCache { get; set; }

        private List<IPackage> FetchOnlineNuGetPackages(SharePointConnection connection)
        {
            var repoUrl = "https://www.myget.org/F/subpointsolutions-ci/api/v2";
            var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);

            return repo.GetPackages().Where(p => p.IsLatestVersion).ToList();
        }

        private List<IPackage> FetchLocalNuGetPackages(SharePointConnection connection)
        {
            var repoUrl = "https://www.myget.org/F/subpointsolutions-ci/api/v2";

            var repo = PackageRepositoryFactory.Default.CreateRepository(repoUrl);
            var result = new List<IPackage>();

            //try
            //{
            //    SPUtils.WithSharePointContext(connection.Url,
            //        connection.UserName,
            //        connection.UserPassword,
            //        context =>
            //        {
            //            var packageManager = new SPMeta2SolutionPackageManager(repo, context);

            //            result = packageManager.LocalRepository
            //                .GetPackages()
            //                .GroupBy(p => p.Id)
            //                .Select(g => g.Where(t => t.IsLatestVersion)
            //                    .OrderByDescending(p => p.Version)
            //                    .FirstOrDefault())
            //                .Where(a => a != null)
            //                .ToList();
            //        });
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.ToString());
            //}

            return result;
        }

        //private void OnSolutionEvent(SolutionEvent obj)
        //{
        //    //switch (obj.EventType)
        //    //{
        //    //    case SolutionEventType.New:
        //    //    case SolutionEventType.Opened:
        //    //    case SolutionEventType.Updated:
        //    //        UpdateSolutionTree(obj.Item);
        //    //        break;
        //    //}
        //}

        private void UpdateSolutionTree(MetaPackSolution solution)
        {
            tvConnections.Nodes.Clear();

            if (solution == null)
            {
                var urls = new string[]
                {
                    "https://portal",
                    "https://portal/departments",
                    "https://portal/departments/it",
                    "https://portal/departments/hr",
                    "https://portal/departments/rnd",
                    "https://portal/departments",
                    "https://portal/search",
                    "https://portal/kb",
                    "https://portal/heldesk",
                    "https://portal/forms-center"
                };

                foreach (var u in urls)
                {
                    var solutionNode = tvConnections.Nodes.Add(u);

                    solutionNode.Tag = u;

                    solutionNode.Nodes.Add("Installed").Tag = "Installed";
                    solutionNode.Nodes.Add("Online").Tag = "Online";

                    //solutionNode.Expand();
                }

                return;
            }

            //foreach (var item in solution.Connections)
            //{
            //    var solutionNode = tvConnections.Nodes.Add(item.Url);

            //    solutionNode.Tag = item;

            //    solutionNode.Nodes.Add("Installed").Tag = "Installed";
            //    solutionNode.Nodes.Add("Online").Tag = "Online";

            //    solutionNode.Expand();
            //}
        }

        private MenuItem BuildMenuItemSeparator()
        {
            var result = new MenuItem
            {
                Text = "-"
            };

            return result;
        }

        private MenuItem BuildMenuItemFromAction(string title, Action action)
        {
            var result = new MenuItem
            {
                Text = title
            };

            result.Click += (s, e) =>
            {
                if (action != null)
                    action.Invoke();
            };

            return result;
        }

        private void ConnectionsContextMenu_Popup(object sender, EventArgs e)
        {
            var menu = ConnectionsContextMenu.MenuItems;

            menu.Clear();

            menu.Add(BuildMenuItemFromAction("Add", () =>
            {
                var control = new SharePointConnectionEditor
                {
                    Text = "Add new SharePoint connection"
                };

                //control.SetEditableObject(new SharePointConnection());

                //ShFormUtils.ShowModal(control,
                //    form =>
                //    {
                //        form.MinimizeBox = false;
                //        form.MaximizeBox = false;
                //        form.ShowIcon = false;
                //        form.ShowInTaskbar = false;
                //    },
                //    (s, ee) =>
                //    {
                //        var c = control.GetEditableObject() as SharePointConnection;
                //        var solution =
                //            ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>().CurrentSolution;

                //        solution.Connections.Add(c);

                //        RaiseEvent<SolutionEvent>(new SolutionEvent
                //        {
                //            EventType = SolutionEventType.Updated,
                //            Item = solution
                //        });
                //    });
            }));

            menu.Add(BuildMenuItemFromAction("Edit", () =>
            {
                var control = new SharePointConnectionEditor
                {
                    Text = "Edit SharePoint connection"
                };

                var seletedNode = tvConnections.SelectedNode;

                if (seletedNode.Tag is SharePointConnection)
                {
                    //control.SetEditableObject(seletedNode.Tag);

                    //ShFormUtils.ShowModal(control,
                    //    form =>
                    //    {
                    //        form.MinimizeBox = false;
                    //        form.MaximizeBox = false;
                    //        form.ShowIcon = false;
                    //        form.ShowInTaskbar = false;
                    //    },
                    //    (s, ee) =>
                    //    {
                    //        var c = control.GetEditableObject() as SharePointConnection;
                    //        var solution =
                    //            ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>().CurrentSolution;

                    //        solution.Connections.Add(c);

                    //        RaiseEvent<SolutionEvent>(new SolutionEvent
                    //        {
                    //            EventType = SolutionEventType.Updated,
                    //            Item = solution
                    //        });
                    //    });
                }
            }));

            if (tvConnections.SelectedNode != null)
            {
                //if (tvConnections.SelectedNode.Tag is SharePointConnection)
                //{
                //    menu.Add(BuildMenuItemSeparator());
                //    menu.Add(BuildMenuItemFromAction("Delete", () =>
                //    {
                //        var seletedNode = tvConnections.SelectedNode;
                //        var connection = seletedNode.Tag as SharePointConnection;

                //        ShFormUtils.Confirm(
                //            string.Format("Are you sure you want to delete connection [{0}]", connection.Url),
                //            () =>
                //            {
                //                var solution =
                //                    ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>().CurrentSolution;

                //                solution.Connections.Remove(connection);
                //                RaiseEvent<SolutionEvent>(new SolutionEvent
                //                {
                //                    EventType = SolutionEventType.Updated,
                //                    Item = solution
                //                });
                //            });
                //    }));
                //}
            }
        }

        private void TvConnections_Click(object sender, EventArgs e)
        {
        }
    }
}