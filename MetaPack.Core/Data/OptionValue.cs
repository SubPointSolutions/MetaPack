using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Data
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
