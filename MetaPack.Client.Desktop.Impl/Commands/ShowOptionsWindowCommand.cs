using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetaPack.Client.Desktop.Impl.Views;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Events.App;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class ShowOptionsWindowCommand : MetaPackDataCommandBase<MetaPackDesktopSettingsViewModel>
    {
        public override void Execute()
        {
            var editorControl = new OptionsViewControl();
            editorControl.ViewModel = Data;

            ShFormUtilsEx.ShowModal(editorControl);
        }
    }
}
