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

        #region methods

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return string.Format("[{0}] - [{1}]", Name, Value);

            return base.ToString();
        }

        #endregion
    }
}
