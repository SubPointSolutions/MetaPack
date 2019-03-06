using MetaPack.Core.Packaging;
using System.Collections.Generic;

namespace MetaPack.Core.Services
{
    /// <summary>
    /// High level abstraction for solution package provisioning service
    /// </summary>
    public abstract class SolutionPackageDeploymentServiceBase
    {
        #region methods
        public abstract void Deploy(SolutionPackageBase solution, IDictionary<string, string> options);

        #endregion
    }
}
