using MetaPack.NuGet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.NuGet.Data;

namespace MetaPack.SPMeta2.Services
{
    public class SPMeta2ToolResolutionService : ToolResolutionServiceBase
    {
        public override List<SolutionToolPackage> GetAdditionalToolPackages()
        {
            var result = new List<SolutionToolPackage>();

            // no additional tools for m2
            // they would be loaded in a runtime as M2 does either CSOM, SSOM or O365 provision

            return result;
        }
    }
}