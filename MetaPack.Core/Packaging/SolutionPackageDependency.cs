using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Packaging
{
    /// <summary>
    /// A high level abstraction for solution package dependency.
    /// Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
    /// </summary>
    [Serializable]
    [DataContract]
    public class SolutionPackageDependency
    {
        public string Id { get; set; }
        public string Version { get; set; }
    }
}
