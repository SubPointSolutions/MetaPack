using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Commands.Base;
using MetaPack.Client.Desktop.Impl.Controls;
using MetaPack.Client.Desktop.Impl.Services;
using MetaPack.Client.Desktop.Impl.ViewModels;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class DeleteNuGetGalleryConnectionCommand : MetaPackDataCommandBase<NuGetGalleryConnectionViewModel>
    {
        public override void Execute()
        {
            AppService.NuGetConnections.Remove(Data);
        }
    }
}
