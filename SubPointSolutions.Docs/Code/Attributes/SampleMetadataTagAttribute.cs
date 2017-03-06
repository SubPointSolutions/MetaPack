using System;

namespace SubPointSolutions.Docs.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class SampleMetadataTagAttribute : Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
