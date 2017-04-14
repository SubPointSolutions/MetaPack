using System;
using System.Runtime.Serialization;

namespace MetaPack.Client.Desktop.Impl.Common
{

    [DataContract]
    public class SharePointConnection : ICloneable
    {
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public SharePointConnectionAuthMode AuthMode { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string UserPassword { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Url))
                return Url;

            return base.ToString();
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
