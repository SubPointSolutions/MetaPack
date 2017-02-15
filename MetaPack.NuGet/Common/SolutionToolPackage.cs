using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.NuGet.Common
{
    [Serializable]
    public class SolutionToolPackage : MarshalByRefObject
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public string AssemblyNameHint { get; set; }
    }
}
