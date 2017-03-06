using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubPointSolutions.Docs.Code.Data
{
    public class SampleMetadata
    {
        public SampleMetadata()
        {
            Resursive = true;
        }

        public string ContentFolderPath { get; set; }
        public string StaticClassName { get; set; }

        public bool Resursive { get; set; }
    }
}
