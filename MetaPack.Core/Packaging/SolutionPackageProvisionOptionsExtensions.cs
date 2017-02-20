using MetaPack.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Packaging
{
    public static class SolutionPackageProvisionOptionsExtensions
    {
        public static OptionValue FindOption(this SolutionPackageProvisionOptions options, string name)
        {
            return options.Options.FirstOrDefault(o => o.Name.ToUpper() == name.ToUpper());
        }

        public static string GetOptionValue(this SolutionPackageProvisionOptions options, string name)
        {
            var option = FindOption(options, name);

            if (option != null)
                return option.Value;

            return null;
        }

        public static SolutionPackageProvisionOptions SetOptionValue(
            this SolutionPackageProvisionOptions options,
            string name, string value)
        {
            var option = FindOption(options, name);

            if (option == null)
            {
                option = new OptionValue
                {
                    Name = name,
                    Value = value
                };

                options.Options.Add(option);
            }

            option.Value = value;

            return options;
        }
    }
}
