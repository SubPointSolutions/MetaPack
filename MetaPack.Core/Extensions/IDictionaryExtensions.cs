using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Extensions
{
    public static class IDictionaryExtensions
    {
        public static string GetOptionValue(this IDictionary<string, string> options, string optionName)
        {
            foreach (var key in options.Keys)
            {
                if (key.ToLower() == optionName.ToLower())
                {
                    return options[key];
                }
            }

            return null;
        }
    }
}
