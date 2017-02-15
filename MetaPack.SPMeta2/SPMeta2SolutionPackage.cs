using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MetaPack.Core;
using MetaPack.Core.Packaging;
//using SPMeta2.Models;

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
            ModelFolders = new List<string>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Path to folders with serialized models
        /// </summary>
        [IgnoreDataMember]
        public List<string> ModelFolders { get; set; }

       
        #endregion

        #region methods

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            if (ModelFolders == null)
                ModelFolders = new List<string>();
        }


        #endregion
    }
}
