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

        public abstract Stream Pack(SolutionPackageBase package);
        public abstract SolutionPackageBase Unpack(Stream package);

        #endregion
    }

    
}
