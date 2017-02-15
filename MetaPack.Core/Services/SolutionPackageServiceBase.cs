using System.IO;
using MetaPack.Core.Packaging;

namespace MetaPack.Core.Services
{
    /// <summary>
    /// High level abstraction for solution package pacakging service
    /// </summary>
    public abstract class SolutionPackageServiceBase
    {
        #region methods

        public abstract Stream Pack(SolutionPackageBase package, SolutionPackageOptions options);
        public abstract SolutionPackageBase Unpack(Stream package, SolutionPackageOptions options);

        #endregion
    }
}
