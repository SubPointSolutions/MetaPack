using Microsoft.SharePoint.Client;
using NuGet;

namespace MetaPack.NuGet.Services
{
    public abstract class MetaPackSolutionPackageManager : PackageManager
    {
        #region constructors
        public MetaPackSolutionPackageManager(IPackageRepository sourceRepository, ClientContext context)
            : this(sourceRepository, new DefaultPackagePathResolver("http://metapack"), new SharePointCSOMFileSystem(context))
        {

        }

        public MetaPackSolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem) :
            base(sourceRepository, pathResolver, fileSystem)
        {
        }

        public MetaPackSolutionPackageManager(IPackageRepository sourceRepository, IPackagePathResolver pathResolver, IFileSystem fileSystem, IPackageRepository localRepository)
            : base(sourceRepository, pathResolver, fileSystem, localRepository)
        {

        }

        #endregion
    }
}
