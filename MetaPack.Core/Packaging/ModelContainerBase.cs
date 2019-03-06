using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Data;

namespace MetaPack.Core.Packaging
{
    [Serializable]
    [DataContract]
    public class ModelContainerBase
    {
        #region constructors

        public ModelContainerBase()
        {
            AdditionalOptions = new List<OptionValue>();
        }

        #endregion

        #region properties

        [DataMember]
        public byte[] Model { get; set; }

        [DataMember]
        public List<OptionValue> AdditionalOptions { get; set; }

        #endregion

        #region methods

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            if (AdditionalOptions == null)
                AdditionalOptions = new List<OptionValue>();
        }

        #endregion
    }
}
