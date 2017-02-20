using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.NuGet.Common
{
    [Serializable]
    public class DeploymentOption : MarshalByRefObject
    {
        #region properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
