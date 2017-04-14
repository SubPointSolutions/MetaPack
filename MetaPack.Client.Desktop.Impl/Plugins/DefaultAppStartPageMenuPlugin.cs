using System;
using System.Collections.Generic;
using System.Diagnostics;
using MetaPack.Client.Desktop.Impl.Commands;
using MetaPack.Client.Desktop.Impl.Views;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Definitions.UI;
using SubPointSolutions.Shelly.Desktop.Plugins;
using SubPointSolutions.Shelly.Desktop.Utils;

namespace MetaPack.Client.Desktop.Impl.Plugins
{
    public class DefaultAppStartPageMenuPlugin : ShAppStartPagePluginBase
    {
        public override List<ShAppStartPageItemDefinition> GetMenuItems()
        {
            var result = new List<ShAppStartPageItemDefinition>();

            // start
            var newSharePointConnection = new ShAppStartPageItemDefinition
            {
                Title = "Add SharePoint connection...",
                Id = new Guid("18281F23-352B-4D7E-86FF-032C2F90A4A6"),
                Order = 100,
                StartsGroup = false,
                Location = "start",
                Click = ShBind.Cmd(MpCommands.Instance.NewSharePointConnection).Bind
            };

            var newNuGetConnection = new ShAppStartPageItemDefinition
            {
                Title = "Add NuGet Gallery connection...",
                Id = new Guid("A06AB89E-6A91-4D0C-A972-F0FA350DA52B"),
                Order = 200,
                StartsGroup = false,
                Location = "start",
                Click = ShBind.Cmd(MpCommands.Instance.NewNuGetGalleryConnection).Bind
            };

            result.Add(newSharePointConnection);
            result.Add(newNuGetConnection);

            // help
            result.Add(new ShAppStartPageItemDefinition
            {
                Title = "MetaPack documentation",
                Id = new Guid("68FD6EEA-D07D-4CAE-930F-0E31FA00C823"),
                Order = 100,
                StartsGroup = false,
                Location = "help",
                Click = (s, e) =>
                {
                    Process.Start("http://docs.subpointsolutions.com/metapack");
                }
            });

            result.Add(new ShAppStartPageItemDefinition
            {
                Title = "Community",
                Id = new Guid("E7213962-F5F4-4B88-BF76-CA4A536F83D1"),
                Order = 200,
                StartsGroup = false,
                Location = "help",
                Click = (s, e) =>
                {
                    Process.Start("https://www.yammer.com/spmeta2feedback");
                }
            });

            result.Add(new ShAppStartPageItemDefinition
            {
                Title = "Report an issue",
                Id = new Guid("C2E50021-E2AB-4036-BAD9-6FBF0C634819"),
                Order = 300,
                StartsGroup = false,
                Location = "help",
                Click = (s, e) =>
                {
                    Process.Start("https://github.com/SubPointSolutions/metapack/issues");
                }
            });

            result.Add(new ShAppStartPageItemDefinition
            {
                Title = "MetaPack CLI",
                Id = new Guid("8D1E3514-465E-4402-90DD-78F7EC0F838D"),
                Order = 400,
                StartsGroup = false,
                Location = "help",
                Click = (s, e) =>
                {
                    Process.Start("http://docs.subpointsolutions.com/metapack/cli");
                }
            });

            result.Add(new ShAppStartPageItemDefinition
           {
               Title = "Add SharePoint connection",
               Id = new Guid("93567388-F3D2-4C55-95A1-E8BC2C4DCF3E"),
               Order = 500,
               StartsGroup = false,
               Location = "help",
               Click = (s, e) =>
               {
                   MpCommands.Instance.NewSharePointConnection();
               }
           });



            return result;
        }
    }
}
