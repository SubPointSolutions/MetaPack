using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Controls;
using SubPointSolutions.Shelly.Desktop.Services;
using SubPointSolutions.Shelly.Desktop.Utils;
using MetaPack.Client.Desktop.Impl.ViewModels;
using MetaPack.Client.Desktop.Impl.Commands.Base;

namespace MetaPack.Client.Desktop.Impl.Commands
{
    public class EditSharePointConnectionCommand : MetaPackDataCommandBase<SharePointConnectionViewModel>
    {
        public override void Execute()
        {
            var editorControl = new SharePointConnectionEditor();

            editorControl.ViewModel = Data;
            ShFormUtilsEx.ShowModal(editorControl);
        }
    }
}
