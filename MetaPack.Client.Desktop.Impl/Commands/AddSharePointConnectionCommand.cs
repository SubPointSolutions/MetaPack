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
using MetaPack.Core.Common;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class NewSharePointConnectionCommand : MetaPackDataCommandBase<SharePointConnectionViewModel>
    {
        public NewSharePointConnectionCommand()
        {

        }

        public override void Execute()
        {
            if (Data == null)
                Data = new SharePointConnectionViewModel();

            var editorControl = new SharePointConnectionEditor();

            editorControl.ViewModel = Data;

            ShFormUtilsEx.ShowModal(new ShowModalExOptions
            {
                Control = editorControl,
                OnOk = (s, e) => { AppService.SharePointConnections.Add(editorControl.ViewModel); }
            });
        }
    }
}
