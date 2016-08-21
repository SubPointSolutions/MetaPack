using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Tests.Utils
{
    public static class EnvironmentUtils
    {
        public static string GetEnvironmentVariable(string varName)
        {
            var variable = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrEmpty(variable))
                variable = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(variable))
                variable = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Process);

            return variable;
        }

        public static IEnumerable<string> GetEnvironmentVariables(string varName)
        {
            var varString = Environment.GetEnvironmentVariable(varName, EnvironmentVariableTarget.Machine);

            return string.IsNullOrEmpty(varString) ?
                    new string[0] :
                    varString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
        }
    }
}
