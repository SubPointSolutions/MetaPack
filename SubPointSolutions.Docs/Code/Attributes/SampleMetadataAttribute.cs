using System;

namespace SubPointSolutions.Docs.Code.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SampleMetadataAttribute : Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }

        // TODO
        //public bool GenerateFullProvisionSample { get; set; }
    }
}
