using System.Runtime.Serialization;

namespace MetaPack.Client.Desktop.Impl.Common
{
    [DataContract]
    public class NuGetGalleryConnection
    {
        public NuGetGalleryConnection()
        {

        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Url { get; set; }


        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Url))
            {
                return string.Format("{0}, {1}", Name, Url);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                return Name;
            }

            if (!string.IsNullOrEmpty(Url))
            {
                return Url;
            }

            return base.ToString();
        }
    }
}
