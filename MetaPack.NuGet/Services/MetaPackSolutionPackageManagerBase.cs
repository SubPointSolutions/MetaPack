using System;
using System.Collections.Generic;
using MetaPack.Core;
using MetaPack.Core.Data;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using MetaPack.NuGet.Data;
using Microsoft.SharePoint.Client;
using NuGet;

namespace MetaPack.NuGet.Services
{
    public abstract class MetaPackSolutionPackageManagerBase : PackageManager
    {
        #region constructors
        public MetaPackSolutionPackageManagerBase(IPackageRepository sourceRepository, ClientContext context)
            : this(sourceRepository, new DefaultPackagePathResolver("http://metapack"), new SharePointCSOMFileSystem(context))
        {
            CurrentClientContext = context;
            CurrentSharePointSiteUrl = context.Url;
        }

        public MetaPackSolutionPackageManagerBase(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem) :
            base(sourceRepository, pathResolver, fileSystem)
        {
            SolutionOptions = new List<OptionValue>();
        }

        public MetaPackSolutionPackageManagerBase(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem, IPackageRepository localRepository)
            : base(sourceRepository, pathResolver, fileSystem, localRepository)
        {
            SolutionOptions = new List<OptionValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// SharePoint client context on which MetaPack should run
        /// </summary>
        protected ClientContext CurrentClientContext { get; private set; }

        /// <summary>
        /// Target SharePoint site url
        /// </summary>
        protected string CurrentSharePointSiteUrl { get; private set; }

        /// <summary>
        /// Suggested, custom tool for solution package operations
        /// </summary>
        public SolutionToolPackage SolutionToolPackage { get; set; }

        /// <summary>
        /// Additional options for solution package operations
        /// </summary>
        public List<OptionValue> SolutionOptions { get; set; }

        #endregion
    }
}
