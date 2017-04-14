using SubPointSolutions.Shelly.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;
using SubPointSolutions.Shelly.Core.Command;

namespace MetaPack.Client.Desktop.Impl.Commands.Base
{
    public class MetaPackCommandBase : CommandBase
    {
        #region propeties

        protected virtual MetaPackDataService AppService
        {
            get
            {
                return ShServiceContainer.Instance.GetAppDataService<MetaPackDataService>();
            }
        }



        #endregion
    }
}
