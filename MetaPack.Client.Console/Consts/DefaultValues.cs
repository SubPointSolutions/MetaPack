using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Client.Console.Consts
{
    public static class DefaultValues
    {
        #region static

        static DefaultValues()
        {
            DefaultNuGetRepositories = new List<string>();
            DefaultNuGetRepositories.Clear();

            DefaultNuGetRepositories.Add("http://metapackgallery.com/api/v2");
        }

        #endregion

        #region properties

        public static List<string> DefaultNuGetRepositories { get; set; }

        #endregion
    }
}
