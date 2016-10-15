using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Common.Commands.Base
{
    public abstract class CommandBase
    {
        #region properties

        public abstract string Name { get; set; }

        public string Url { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        public bool IsSharePointOnline { get; set; }

        #endregion

        #region methods

        public abstract object Execute();

        #endregion

    }
}
