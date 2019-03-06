using System.Linq;

namespace MetaPack.NuGet.Data
{
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
