using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Services;
using NuGet;
using MetaPack.Core.Consts;
using MetaPack.Core.Utils;

namespace MetaPack.SPMeta2.Services
{
    /// <summary>
    /// Solution packaging service implementation for SPMeta2 models
    /// </summary>
    public class SPMeta2SolutionPackageService : NuGetSolutionPackageService
    {
        #region contructors
        public SPMeta2SolutionPackageService()
        {
            ModelFoldersPath = "Models";
        }

        #endregion

        #region properties

        public string ModelFoldersPath { get; set; }

        #endregion

        #region methods

        

      
        #endregion
    }
}
