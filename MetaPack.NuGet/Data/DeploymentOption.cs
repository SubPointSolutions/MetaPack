using System;

namespace MetaPack.NuGet.Data
{
    [Serializable]
    public class DeploymentOption : MarshalByRefObject
    {
        #region properties

        public string Name { get; set; }
        public string Value { get; set; }

        #endregion
    }
}
