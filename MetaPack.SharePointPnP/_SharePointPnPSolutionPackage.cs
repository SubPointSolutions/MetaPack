using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Common;
using MetaPack.Core.Packaging;

namespace MetaPack.SharePointPnP
{
    //[Serializable]
    //[DataContract]
    //public class _SharePointPnPSolutionPackage : SolutionPackageBase
    //{
    //    #region constructors

    //    public _SharePointPnPSolutionPackage()
    //    {
    //        ProvisioningTemplateFolders = new List<string>();
    //        ProvisioningTemplateOpenXmlPackageFolders = new List<string>();

    //        AdditionalOptions.Add(new OptionValue
    //        {
    //            Name = DefaultOptions.SolutionToolPackage.PackageId.Id,
    //            Value = "MetaPack.SharePointPnP"
    //        });
    //    }

    //    #endregion

    //    #region properties

    //    /// <summary>
    //    /// Paths to folders with provisioning templates.
    //    /// Each folder represents a separate provisioning template with all required files, assets and so on
    //    /// </summary>
    //    [IgnoreDataMember]
    //    public List<string> ProvisioningTemplateFolders { get; set; }

    //    /// <summary>
    //    /// Paths to folders with provisioning templates packaged as OpenXML files.
    //    /// Each folder might have one or more OpenXML PnP package (.pnp file)
    //    /// </summary>
    //    [IgnoreDataMember]
    //    public List<string> ProvisioningTemplateOpenXmlPackageFolders { get; set; }

    //    #endregion

    //    #region methods

    //    [OnDeserializing]
    //    private void OnDeserializing(StreamingContext context)
    //    {
    //        if (ProvisioningTemplateFolders == null)
    //            ProvisioningTemplateFolders = new List<string>();

    //        if (ProvisioningTemplateOpenXmlPackageFolders == null)
    //            ProvisioningTemplateOpenXmlPackageFolders = new List<string>();
    //    }


    //    #endregion
    //}
}