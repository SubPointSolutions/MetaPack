using System;
using System.Windows.Forms;
using MetaPack.Client.Desktop.Impl.Commands;
using MetaPack.Client.Desktop.Impl.Controls;
using MetaPack.Client.Desktop.Impl.Views;
using SubPointSolutions.Shelly.Core.Extensibility;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Definitions.UI;
using SubPointSolutions.Shelly.Desktop.Events.App;
using SubPointSolutions.Shelly.Desktop.Events.UI;

namespace MetaPack.Client.Desktop.Impl.Plugins
{
    public class DefaultAppViewsPlugin : ShPluginBase
    {
        public override void Init()
        {
            ReceiveEvent<ShOnAppLoadCompletedEvent>(OnAppLoad);
        }

        private void OnAppLoad(ShOnAppLoadCompletedEvent obj)
        {
            RaiseEvent(new ShAddAppViewWindowEvent
            {
                ViewWindowControl = new ShAppViewWindowControlDefinition
                {
                    Id = new Guid("BFE433EC-2234-426F-8F76-0815A08F566B"),
                    Title = "Home",
                    AssemblyQualifiedName = typeof(StartPageViewControl).AssemblyQualifiedName
                }
            });

            RaiseEvent(new ShAddAppViewWindowEvent
            {
                ViewWindowControl = new ShAppViewWindowControlDefinition
                {
                    Id = new Guid("145EFE42-A240-49FD-8541-4145AD16D3FF"),
                    Title = "Installed packages",
                    AssemblyQualifiedName = typeof(InstalledPackagesViewControl).AssemblyQualifiedName
                }
            });

            RaiseEvent(new ShAddAppViewWindowEvent
            {
                ViewWindowControl = new ShAppViewWindowControlDefinition
                {
                    Id = new Guid("0A8401EE-A00A-4101-9E9E-20C2332FE294"),
                    Title = "Available packages",
                    AssemblyQualifiedName = typeof(AvailablePackagesViewControl).AssemblyQualifiedName
                }
            });

            RaiseEvent(new ShAddAppViewWindowEvent
            {
                ViewWindowControl = new ShAppViewWindowControlDefinition
                {
                    Id = new Guid("0A8401EE-A00A-4101-9E9E-20C2332FE294"),
                    Title = "Options",
                    //AssemblyQualifiedName = typeof(WfOutputControl).AssemblyQualifiedName,
                    Click = (sender, args) =>
                    {
                        MpCommands.Instance.ShowOptionsWindow();
                        args.Cancel = true;
                    }
                }
            });

            RaiseEvent(new ShAddAppViewWindowEvent
            {
                ViewWindowControl = new ShAppViewWindowControlDefinition
                {
                    Id = new Guid("0A8401EE-A00A-4101-9E9E-20C2332FE294"),
                    Title = "About",
                    //AssemblyQualifiedName = typeof(WfOutputControl).AssemblyQualifiedName,
                    Click = (sender, args) =>
                    {
                        MpCommands.Instance.ShowAboutWindow();
                        args.Cancel = true;
                    }
                }
            });
        }
    }
}