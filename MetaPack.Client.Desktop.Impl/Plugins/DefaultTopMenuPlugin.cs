using System;
using MetaPack.Client.Desktop.Impl.Commands;
using SubPointSolutions.Shelly.Core.Extensibility;
using SubPointSolutions.Shelly.Core.Utils;
using SubPointSolutions.Shelly.Desktop.Consts;
using SubPointSolutions.Shelly.Desktop.Definitions.Base;
using SubPointSolutions.Shelly.Desktop.Definitions.UI;
using SubPointSolutions.Shelly.Desktop.Events.App;
using SubPointSolutions.Shelly.Desktop.Events.UI;

namespace MetaPack.Client.Desktop.Impl.Plugins
{
    public class DefaultTopMenuPlugin : ShPluginBase
    {
        public override void Init()
        {
            ReceiveEvent<ShOnAppLoadCompletedEvent>(AddDefaultAppMenu);
        }

        protected void AddTopMenu(ShAppMenuItemDefinitionBase menu)
        {
            RaiseEvent(new ShAddAppTopMenuItemEvent
            {
                MenuItem = menu
            });
        }

        private void AddDefaultAppMenu(ShOnAppLoadCompletedEvent obj)
        {
            AddFileMenu(ShAppMenuIds.File);
            AddViewMenu(ShAppMenuIds.View);
            AddToolsMenu(ShAppMenuIds.Tools);
            AddHelpMenu(ShAppMenuIds.Help);


        }

        private void AddViewMenu(Guid rootMenuId)
        {
            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = rootMenuId,
                Title = "Views",
                Order = 50
            });

            //AddTopMenu(new ShAppTopMenuItemDefinition
            //{
            //    Id = new Guid("D2E3B500-7C10-4B45-AE7E-B56165B93979"),
            //    ParentItemId = rootMenuId,
            //    Title = "Output",
            //    Click = ShBind.Cmd(MpCommands.Instance.ShowOutput).Bind,
            //    Order = 50
            //});
        }

        private void AddHelpMenu(Guid rootMenuId)
        {
            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = rootMenuId,
                Title = "Help",
                Order = 50
            });


            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = new Guid("541FF399-99E1-4950-ACF0-017607BF2D32"),
                ParentItemId = rootMenuId,
                Title = "About",
                Click = ShBind.Cmd(MpCommands.Instance.ShowAboutWindow).Bind,
                Order = 50
            });
        }

        private void AddToolsMenu(Guid rootMenuId)
        {
            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = rootMenuId,
                Title = "Tools",
                Order = 10
            });

            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = new Guid("9DAB21BD-72FC-4E68-9457-4E417FC1DD3E"),
                ParentItemId = rootMenuId,
                Title = "Options",
                Click = ShBind.Cmd(MpCommands.Instance.ShowOptionsWindow).Bind,
                Order = 50
            });
        }

        private void AddFileMenu(Guid rootMenuId)
        {
            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = rootMenuId,
                Title = "File",
                Order = 10
            });

            //AddTopMenu(new ShAppTopMenuItemDefinition
            //{
            //    Id = new Guid("11BC8EBB-322D-4156-8B02-34D360FB073A"),
            //    ParentItemId = rootMenuId,
            //    Title = "New",
            //    Click = ShBind.Cmd(MpCommands.Instance.NewSolution).Bind,
            //    Order = 50
            //});

            //AddTopMenu(new ShAppTopMenuItemDefinition
            //{
            //    Id = new Guid("FF910BFF-1BA9-4988-9FC0-705667258BDE"),
            //    ParentItemId = rootMenuId,
            //    Title = "Open",
            //    Click = ShBind.Cmd(MpCommands.Instance.OpenSolution).Bind,
            //    Order = 60
            //});


            //AddTopMenu(new ShAppTopMenuItemDefinition
            //{
            //    Id = new Guid("FD7533F8-0267-4FC4-8603-8C4E8E5F1E34"),
            //    ParentItemId = rootMenuId,
            //    Title = "Save",
            //    StartsGroup = true,
            //    Click = ShBind.Cmd(MpCommands.Instance.SaveSolution).Bind,
            //    Order = 60
            //});


            //AddTopMenu(new ShAppTopMenuItemDefinition
            //{
            //    Id = new Guid("4E5B5FA7-36B8-44E7-9461-717C2AB69DE0"),
            //    ParentItemId = rootMenuId,
            //    Title = "Save As...",
            //    Click = ShBind.Cmd(MpCommands.Instance.SaveSolutionAs).Bind,
            //    Order = 60
            //});

            AddTopMenu(new ShAppTopMenuItemDefinition
            {
                Id = new Guid("98788738-1ACB-41EF-A680-674C393A9E11"),
                ParentItemId = rootMenuId,
                StartsGroup = true,
                Title = "Exit",
                Click = ShBind.Cmd(MpCommands.Instance.ExitApp).Bind,
                Order = 150
            });
        }
    }
}
