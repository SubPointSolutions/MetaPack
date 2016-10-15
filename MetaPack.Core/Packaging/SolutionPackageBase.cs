using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MetaPack.Core.Packaging
{
    /// <summary>
    /// A high level abstraction for solution package.
    /// Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
    /// </summary>
    [Serializable]
    [DataContract]
    public class SolutionPackageBase
    {
        #region constructors

        public SolutionPackageBase()
        {
            Dependencies = new List<SolutionPackageDependency>();
        }

        #endregion

        #region properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Authors { get; set; }

        [DataMember]
        public string Company { get; set; }

        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public string Owners { get; set; }

        [DataMember]
        public string ReleaseNotes { get; set; }

        [DataMember]
        public string Summary { get; set; }

        [DataMember]
        public string ProjectUrl { get; set; }

        [DataMember]
        public string IconUrl { get; set; }

        [DataMember]
        public string LicenseUrl { get; set; }

        [DataMember]
        public string Copyright { get; set; }

        [DataMember]
        public string Tags { get; set; }

        [DataMember]
        public List<SolutionPackageDependency> Dependencies { get; set; }

        #endregion
    }
}
