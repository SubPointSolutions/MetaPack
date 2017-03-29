using SubPointSolutions.Shelly.Desktop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Client.Desktop.Impl.Services;
using SubPointSolutions.Shelly.Core;

namespace MetaPack.Client.Desktop.Impl.Commands.Base
{
    public class MetaPackDataCommandBase<TDataModel> : MetaPackCommandBase
    {
        #region propeties

        public virtual TDataModel Data { get; set; }

        #endregion
    }
}
