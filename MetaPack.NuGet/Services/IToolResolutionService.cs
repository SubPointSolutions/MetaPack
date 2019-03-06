using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.NuGet.Data;

namespace MetaPack.NuGet.Services
{
    public abstract class ToolResolutionServiceBase
    {
        public abstract List<SolutionToolPackage> GetAdditionalToolPackages();
    }
}
