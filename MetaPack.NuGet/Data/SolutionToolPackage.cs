using System;

namespace MetaPack.NuGet.Data
{
    [Serializable]
    public class SolutionToolPackage : MarshalByRefObject
    {
        public string Id { get; set; }
        public string Version { get; set; }

        public string AssemblyNameHint { get; set; }
    }
}
