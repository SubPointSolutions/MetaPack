using MetaPack.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using MetaPack.Core.Utils;

namespace MetaPack.Core.Packaging
{
    /// <summary>
    /// A high level abstraction for solution package.
    /// Follows NuGet spec design - https://docs.nuget.org/ndocs/schema/nuspec
    /// 
    /// Solution package is a container for SERIALIZED models.
    /// It means that solution package does not depend on a particular API/assembly preferring adding / finding models in serialazable, platform and api independent way.
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

            if (_models == null)
                _models = new List<ModelContainerBase>();
        }

        #endregion

        #region methods

        private List<ModelContainerBase> _models = new List<ModelContainerBase>();

        public virtual IEnumerable<ModelContainerBase> GetModels()
        {
            // sort by the order
            return _models.OrderBy(m =>
            {
                var result = -1;

                var orderOption =
                    m.AdditionalOptions.FirstOrDefault(v => v.Name.ToUpper() == DefaultOptions.Model.Order.Id.ToUpper());

                if (orderOption != null)
                {
                    var tmpInt = ConvertUtils.ToInt(orderOption.Value);

                    if (tmpInt.HasValue)
                        result = tmpInt.Value;
                }

                return result;
            });
        }

        public virtual void AddModel(ModelContainerBase modelContainer)
        {
            AddModelInternal(modelContainer);
        }

        public virtual void RemoveModel(ModelContainerBase modelContainer)
        {
            if (_models.Contains(modelContainer))
                _models.Remove(modelContainer);
        }

        protected virtual void AddModelInternal(ModelContainerBase modelContainer)
        {
            ProcessModelContainerMetadata(modelContainer);

            _models.Add(modelContainer);
        }

        private void ProcessModelContainerMetadata(ModelContainerBase modelContainer)
        {
            // TODO
            var isRestoreOperation = false;

            if (isRestoreOperation)
            {
                // nothing
            }
            else
            {
                // fill out MD5 and other things
            }
        }

        #endregion

    }
}
