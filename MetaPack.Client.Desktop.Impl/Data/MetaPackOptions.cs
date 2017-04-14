using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using MetaPack.Client.Desktop.Impl.Common;
using SubPointSolutions.Shelly.Desktop.Attributes;

namespace MetaPack.Client.Desktop.Impl.Data
{
    [DataContract]
    [DisplayName("MetaPack")]
    public class MetaPackOptions
    {
        public MetaPackOptions()
        {
            NuGetOptions = new NuGetOptions();
            GeneralOptions = new GeneralOptions();
        }

        #region prop

        [DataMember]
        [DisplayName("NuGet Galleries,2")]
        [ShEntityEditorControl(
             AssemblyQualifiedName =
                 "MetaPackGUI.Impl.Controls.NuGetOptionsEditor, MetaPackGUI.Impl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
         )]
        public NuGetOptions NuGetOptions { get; set; }

        [DataMember]
        [DisplayName("General,1")]
        public GeneralOptions GeneralOptions { get; set; }

        #endregion
    }

    [DataContract]
    public class GeneralOptions
    {
        [DataMember]
        public int ConnectionTimeout { get; set; }

        [DataMember]
        public string SomethingElse { get; set; }

        [DataMember]
        public bool Yes { get; set; }
    }

    [DataContract]
    public class NuGetOptions
    {
        public NuGetOptions()
        {
            Connections = new List<NuGetGalleryConnection>();
        }

        [DataMember]
        public List<NuGetGalleryConnection> Connections { get; set; }
    }
}