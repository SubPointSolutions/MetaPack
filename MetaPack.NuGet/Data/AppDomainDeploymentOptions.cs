using System;
using System.Collections.Generic;

namespace MetaPack.NuGet.Data
{
    [Serializable]
    public class AppDomainDeploymentOptions : MarshalByRefObject
    {
        #region contrustors

        public AppDomainDeploymentOptions()
        {
            AdditionalOptions = new List<DeploymentOption>();
            ToolAdditionalPackages = new List<SolutionToolPackage>();
            AssemblyProbingPaths = new List<string>();
        }

        #endregion

        #region properties

        public string PackageFilePath { get; set; }

        public string PackagingServiceClassFullName { get; set; }
        public string DeploymentServiceClassFullName { get; set; }

        public SolutionToolPackage ToolMainPackage { get; set; }
        public List<SolutionToolPackage> ToolAdditionalPackages { get; set; }

        public List<DeploymentOption> AdditionalOptions { get; set; }
        public List<string> AssemblyProbingPaths { get; set; }

        #endregion
    }

  
}
