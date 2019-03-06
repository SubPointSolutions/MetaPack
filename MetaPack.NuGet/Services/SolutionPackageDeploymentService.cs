using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AppDomainToolkit;
using MetaPack.Core.Services;
using NuGet;
using MetaPack.Core.Packaging;
using MetaPack.NuGet.Data;

namespace MetaPack.NuGet.Services
{
    public class SolutionPackageDeploymentOpetion
    {
        public SolutionPackageDeploymentOpetion()
        {
            ToolPackages = new List<string>();
        }

        public string ProvisionClassName { get; set; }

        public List<string> ToolPackages { get; set; }

        public string ToolPackageName { get; set; }
    }

    public abstract class SolutionPackageDeploymentService : SolutionPackageDeploymentServiceBase
    {
        #region constructors

        public SolutionPackageDeploymentService()
        {

        }
        #endregion

        public virtual IEnumerable<SolutionToolPackage> GetAdditionalToolPackages(SolutionPackageBase solutionPackage, IDictionary<string, string> options)
        {
            return Enumerable.Empty<SolutionToolPackage>();
        }
    }
}
