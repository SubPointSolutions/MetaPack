using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Controls;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class EditNuGetGalleryConnectionCommand : MetaPackDataCommandBase<NuGetGalleryConnectionViewModel>
    {
        public override void Execute()
        {
            var editorControl = new NuGetConnectionEditor();

            editorControl.ViewModel = Data;
            ShFormUtilsEx.ShowModal(editorControl);
        }
    }
}
