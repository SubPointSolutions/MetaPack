using System.Linq;

namespace MetaPack.SPMeta2.Extensions
{
    public static class ModelNodeExtensions
    {
        #region methods


        #endregion

        #region utils

        //private static ModelNode SetValue(this ModelNode modelNode, string name, string value)
        //{
        //    var propertyBag = modelNode.PropertyBag.FirstOrDefault(v => v.Name.ToUpper() == name.ToUpper());

        //    if (propertyBag == null)
        //    {
        //        propertyBag = new PropertyBagValue { Name = name, Value = value };

        //        modelNode.PropertyBag.Add(propertyBag);
        //    }

        //    propertyBag.Value = value;

        //    return modelNode;
        //}

        //private static string GetValue(this ModelNode modelNode, string name)
        //{
        //    var propertyBag = modelNode.PropertyBag.FirstOrDefault(v => v.Name.ToUpper() == name.ToUpper());

        //    if (propertyBag != null)
        //        return propertyBag.Value;

        //    return null;
        //}

        #endregion
    }
}
