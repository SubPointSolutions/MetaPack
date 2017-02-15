using MetaPack.Core.Common;
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
            AdditionalOptions = new List<OptionValue>();
        }

        #endregion

        #region properties

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Authors { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Company { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Owners { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Summary { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string ProjectUrl { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string IconUrl { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string LicenseUrl { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Copyright { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public string Tags { get; set; }

        /// <summary>
        /// Corresponds to NuGet package spec design 
        /// https://docs.nuget.org/ndocs/schema/nuspec
        /// </summary>
        [DataMember]
        public List<SolutionPackageDependency> Dependencies { get; set; }

        #endregion

        #region additional props
        /// <summary>
        /// Additional data accosiated with the solution package
        /// </summary>
        [DataMember]
        public List<OptionValue> AdditionalOptions { get; set; }

        #endregion

        #region methods

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            if (AdditionalOptions == null)
                AdditionalOptions = new List<OptionValue>();

            if (Dependencies == null)
                Dependencies = new List<SolutionPackageDependency>();
        }

        #endregion
    }
}
