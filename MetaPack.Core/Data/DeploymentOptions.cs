using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.NuGet.Common
{
    [Serializable]
    public class DeploymentOptions : MarshalByRefObject
    {
        #region contrustors

        public DeploymentOptions()
        {
            AdditionalOptions = new List<DeploymentOption>();
            ToolAdditionalPackages = new List<ToolPackage>();
        }

        #endregion

        #region properties

        public string PackageFilePath { get; set; }

        public string PackagingServiceClassFullName { get; set; }
        public string DeploymentServiceClassFullName { get; set; }

        public ToolPackage ToolMainPackage { get; set; }
        public List<ToolPackage> ToolAdditionalPackages { get; set; }

        public List<DeploymentOption> AdditionalOptions { get; set; }

        #endregion
    }

    public static class DeploymentOptionsExtensions
    {
        public static DeploymentOptions Add(this DeploymentOptions options, string name, string value)
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

        public static string GetValue(this DeploymentOptions options, string name)
        {
            var option = options.AdditionalOptions.FirstOrDefault(o => o.Name.ToUpper() == name.ToUpper());

            if (option != null)
                return option.Value;

            return null;
        }

        public static IEnumerable<string> GetValues(this DeploymentOptions options, string name)
        {
            var value = GetValue(options, name);

            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(';');
            }

            return Enumerable.Empty<String>();
        }
    }
}
