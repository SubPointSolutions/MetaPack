using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MetaPack.Client.Desktop.Impl.Data
{
    [DataContract]
    public class MetaPackSolution
    {
        #region constructors

        public MetaPackSolution()
        {
            //Connections = new List<SharePointConnection>();
        }

        #endregion

        #region properties

        //[DataMember]
        //public List<SharePointConnection> Connections { get; set; }

        #endregion
    }
}
