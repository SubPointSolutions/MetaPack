using System;
using SPMeta2.Models;

namespace MetaPack.SPMeta2
{
    public static class SPMeta2SolutionPackageExtensions
    {
        #region methods

        public static SPMeta2SolutionPackage AddModel(this SPMeta2SolutionPackage package, ModelNode model)
        {
            return AddModel(package, model, null);
        }

        public static SPMeta2SolutionPackage AddModel(this SPMeta2SolutionPackage package, ModelNode model, Action<ModelNode> action)
        {
            if (action != null)
                action(model);

            package.Models.Add(model);

            return package;
        }

        #endregion
    }
}
