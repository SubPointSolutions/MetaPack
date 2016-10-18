using System;
using System.Diagnostics;
using System.Linq;
using MetaPack.Client.Common.Commands.Base;
using MetaPack.Client.Common.Services;
using MetaPack.SPMeta2.Services;
using Microsoft.SharePoint.Client;
using NuGet;
using System.IO;
using MetaPack.NuGet.Services;

namespace MetaPack.Client.Common.Commands
{
    public class DefaultNuGetPushCommand : CommandBase
    {
        #region properties
        public override string Name
        {
            get { return "push"; }
            set
            {

            }
        }

        public string Source { get; set; }
        public string ApiKey { get; set; }

        public Stream Package { get; set; }

        #endregion

        #region methods
        public override object Execute()
        {
            if (string.IsNullOrEmpty(Source))
                throw new ArgumentException("Source");

            if (string.IsNullOrEmpty(ApiKey))
                throw new ArgumentException("ApiKey");

            if (Package == null)
                throw new ArgumentException("Package");

            var packagingService = new SPMeta2SolutionPackageService();
            packagingService.Push(Package, Source, ApiKey);

            return null;
        }

        #endregion
    }
}
