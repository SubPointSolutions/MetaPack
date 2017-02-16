using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Exceptions
{
    [Serializable]
    public class MetaPackException : Exception
    {
        public MetaPackException() { }
        public MetaPackException(string message) : base(message) { }
        public MetaPackException(string message, Exception inner) : base(message, inner) { }
        protected MetaPackException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
