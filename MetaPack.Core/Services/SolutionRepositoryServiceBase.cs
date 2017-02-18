using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaPack.Core.Services
{
    public class SolutionRepositoryServiceBase
    {
        #region methods

        public virtual IEnumerable<Stream> GetSolutionPackageStreams()
        {
            return Enumerable.Empty<Stream>();
        }

        #endregion
    }
}
