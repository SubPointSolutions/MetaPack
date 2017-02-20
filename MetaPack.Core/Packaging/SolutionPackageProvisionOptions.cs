using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetaPack.Core.Common;

namespace MetaPack.Core.Packaging
{
    /// <summary>
    /// Defines context for the solution package provision
    /// </summary>
    public class SolutionPackageProvisionOptions
    {
        #region constructors

        public SolutionPackageProvisionOptions()
        {
            Options = new List<OptionValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Target solution package to be deployed
        /// </summary>
        public SolutionPackageBase SolutionPackage { get; set; }

        /// <summary>
        /// Additional options for the provisioning service.
        /// Essentially, key-calue pairs
        /// </summary>
        public List<OptionValue> Options { get; set; }

        #endregion
    }

    
}
