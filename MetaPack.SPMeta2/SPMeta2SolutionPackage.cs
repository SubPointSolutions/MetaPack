using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MetaPack.Core;
using MetaPack.Core.Packaging;
using SPMeta2.Models;

namespace MetaPack.SPMeta2
{
    /// <summary>
    /// A high level abstraction for SPMeta2 package.
    /// </summary>
    [Serializable]
    [DataContract]
    public class SPMeta2SolutionPackage : SolutionPackageBase
    {
        #region constructors

        public SPMeta2SolutionPackage()
        {
            Models = new List<ModelNode>();
        }

        #endregion

        #region properties

        [DataMember]
        public List<ModelNode> Models { get; set; }

        #endregion
    }
}
