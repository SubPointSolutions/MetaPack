using System;
using System.Runtime.Serialization;

namespace MetaPack.Core.Common
{
    [Serializable]
    [DataContract]
    public class OptionValue
    {
        #region properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        #endregion
    }
}
