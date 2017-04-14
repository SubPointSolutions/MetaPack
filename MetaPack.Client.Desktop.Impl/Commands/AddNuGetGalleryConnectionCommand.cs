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
using System.Windows.Forms;
using Quark.Desktop.Controls;
using SubPointSolutions.Shelly.Desktop.Controls;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class AddNuGetGalleryConnectionCommand : MetaPackDataCommandBase<NuGetGalleryConnectionViewModel>
    {
        public AddNuGetGalleryConnectionCommand()
        {

        }

        public override void Execute()
        {
            if (Data == null)
                Data = new NuGetGalleryConnectionViewModel();

            var editorControl = new NuGetConnectionEditor();

            editorControl.ViewModel = Data;

            ShFormUtilsEx.ShowModal(new ShowModalExOptions
            {
                Control = editorControl,
                OnOk = (s, e) => { AppService.NuGetConnections.Add(editorControl.ViewModel); }
            });
        }
    }
}
