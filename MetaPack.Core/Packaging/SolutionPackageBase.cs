using System;
using System.Runtime.Serialization;

namespace MetaPack.Core.Packaging
{
    /// <summary>
    /// A high level abstraction for solution package.
    /// Solution package mostly follows NuGet spec design - https://docs.nuget.org/create/nuspec-reference
    /// </summary>
    [Serializable]
    [DataContract]
    public class SolutionPackageBase
    {
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

        #endregion
    }
}
