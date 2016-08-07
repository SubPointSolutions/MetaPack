using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;

namespace MetaPack.Core.Services
{
    public static class SolutionPackageServiceBaseExtensions
    {
        #region methods
        public static Stream Pack(this SolutionPackageServiceBase service, SolutionPackageBase package)
        {
            return service.Pack(package, null);
        }

        public static void PackToFile(this SolutionPackageServiceBase service, SolutionPackageBase package, string filePath)
        {
            var packageStream = service.Pack(package, null);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                packageStream.CopyTo(fileStream);
            }
        }

        public static SolutionPackageBase Unpack(this SolutionPackageServiceBase service, Stream package)
        {
            return service.Unpack(package, null);
        } 
        #endregion
    }
}
