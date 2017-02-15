using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.NuGet.Common
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

    public static class AppDomainDeploymentOptionsExtensions
    {
        public static AppDomainDeploymentOptions Add(this AppDomainDeploymentOptions options, string name, string value)
        {
            var option = options.AdditionalOptions.FirstOrDefault(o => o.Name.ToUpper() == name.ToUpper());

            if (option == null)
            {
                option = new DeploymentOption
                {
                    Name = name,
                    Value = value
                };
            }

            option.Value = value;

            return options;
        }

        public static string GetValue(this AppDomainDeploymentOptions options, string name)
        {
            var option = options.AdditionalOptions.FirstOrDefault(o => o.Name.ToUpper() == name.ToUpper());

            if (option != null)
                return option.Value;

            return null;
        }

    }
}
