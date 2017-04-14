using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetaPack.Client.Desktop.Impl.Views;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class ShowNuGetPackageDetailsCommand : MetaPackDataCommandBase<NuGetPackageViewModel>
    {
        public override void Execute()
        {
            var editorControl = new PackageDetailsView();
            editorControl.ViewModel = Data;

            ShFormUtilsEx.ShowModal(editorControl);
        }
    }
}
