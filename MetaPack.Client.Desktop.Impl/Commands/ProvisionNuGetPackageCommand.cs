using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Common.Commands;
using MetaPack.Client.Desktop.Impl.Common;
using MetaPack.Client.Desktop.Impl.Controls;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetaPack.Client.Desktop.Impl.Views;
using MetaPack.Core;
using MetaPack.Core.Services;
using NuGet;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using MetaPack.Client.Desktop.Impl.Commands.Base;
using SubPointSolutions.Shelly.Desktop.Controls;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class ProvisionNuGetPackageCommand : MetaPackDataCommandBase<NuGetPackageViewModel>
    {
        #region properties

        protected virtual MetaPackUITraceService TraceService
        {
            get
            {
                return MetaPackServiceContainer.Instance.GetService<TraceServiceBase>() as MetaPackUITraceService;
            }
        }

        private PackageDeploymentView editorControl { get; set; }

        private EventHandler<TraceServiceEventArgs> onLogHandler;

        #endregion

        public override void Execute()
        {
            editorControl = new PackageDeploymentView();
            onLogHandler = (ss, ee) => { editorControl.ReportProgress(ee.Message, 1, 10); };

            var model = new NuGetPackageDeploymentViewModel
            {
                SharePointConnection = AppService.ActiveSharePointConnection,
                NuGetPackage = Data
            };

            editorControl.ViewModel = model;

            ShFormUtilsEx.ShowModal(new ShowModalExOptions
            {
                Control = editorControl,
                OnControlSetup = (control) => { control.OkButton.Text = "Deploy"; },
                OnOk = (s, e) =>
                {
                    e.Cancel = true;
                    PerformPackageDeploymentAsync(e, model);
                }
            });
        }

        private void PerformPackageDeploymentAsync(ShModalDialogControlClosingEventArgs e, NuGetPackageDeploymentViewModel model)
        {
            TraceService.OnTrace += onLogHandler;

            ShAsync.DoAsync(new ShAsyncOptions
            {
                Do = () =>
                {
                    var count = 10;

                    e.Control.TsEnableOkButton(false);
                    e.Control.TsEnableCancelButton(false);

                    try
                    {
                        DeployNuGetPackageInternal(
                            model.SharePointConnection.Dto,
                            model.NuGetPackage.Package,
                            model.IsForceDeploy);
                    }
                    finally
                    {
                        e.Control.TsEnableOkButton(true);
                        e.Control.TsEnableCancelButton(true);

                        TraceService.OnTrace += onLogHandler;
                    }
                },
                OnDone = () =>
                {
                    TraceService.OnTrace += onLogHandler;
                }
            });
        }

        internal void DeployNuGetPackageInternal(SharePointConnection connection, IPackage package, bool force)
        {
            var appDataServices = ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();

            var command = new DefaultInstallCommand();

            command.Url = connection.Url;

            if (connection.AuthMode.Id == SharePointConnectionAuthMode.SharePointOnline.Id)
            {
                command.SharePointVersion = "o365";
                command.SharePointApi = "csom";

                command.UserName = connection.UserName;
                command.UserPassword = connection.UserPassword;
            }
            else if (connection.AuthMode.Id == SharePointConnectionAuthMode.WindowsAuthentication.Id)
            {
                command.SharePointVersion = "sp2013";
                command.SharePointApi = "csom";

                if (!string.IsNullOrEmpty(connection.UserName)
                    && !string.IsNullOrEmpty(connection.UserPassword))
                {
                    command.UserName = connection.UserName;
                    command.UserPassword = connection.UserPassword;

                }
            }

            command.PackageSources = appDataServices.NuGetConnections.Select(s => s.Url).ToList();
            command.Id = package.Id;
            command.Version = package.Version.ToString();
            command.PreRelease = package.IsReleaseVersion();

            command.Force = force;

            command.Execute();
        }
    }
}
