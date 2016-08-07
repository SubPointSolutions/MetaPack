using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetaPack.Core.Packaging;

namespace MetaPack.Core.Services
{
    public interface ISolutionPackageSerializationService
    {
        string SerializeSolutionPackage(SolutionPackageBase package);

        SolutionPackageBase DeserializeSolutionPackage(string value);
    }
}
