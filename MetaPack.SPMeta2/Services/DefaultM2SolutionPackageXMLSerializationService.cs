using MetaPack.Core;
using MetaPack.Core.Packaging;
using MetaPack.Core.Services;
using SPMeta2.Definitions;
using SPMeta2.Extensions;
using SPMeta2.Models;
using SPMeta2.Utils;

namespace MetaPack.SPMeta2.Services
{
    internal static class DefaultXMLSerializationServiceExtensions
    {
        public static string SerializeSolutionPackage(this DefaultXMLSerializationService service, SolutionPackageBase package)
        {
            service.EnsureKnownTypes(package as SPMeta2SolutionPackage);

            return service.Serialize(package);
        }

        public static SolutionPackageBase DeserializeSolutionPackage(this DefaultXMLSerializationService service, string value)
        {
            service.RegisterKnownType(typeof(SPMeta2SolutionPackage));

            var defs = ReflectionUtils.GetTypesFromAssembly<DefinitionBase>(typeof(FieldDefinition).Assembly);
            var nodes = ReflectionUtils.GetTypesFromAssembly<ModelNode>(typeof(FieldDefinition).Assembly);

            foreach (var t in defs)
                service.RegisterKnownType(t);

            foreach (var t in nodes)
                service.RegisterKnownType(t);

            return service.Deserialize(typeof(SPMeta2SolutionPackage), value) as SPMeta2SolutionPackage;
        }

        private static void EnsureKnownTypes(this DefaultXMLSerializationService service, SolutionPackageBase p)
        {
            var package = p as SPMeta2SolutionPackage;

            service.RegisterKnownType(package.GetType());

            foreach (var model in package.Models)
            {
                var allModelNodes = model.FindNodes(n => true);

                foreach (var node in allModelNodes)
                {
                    service.RegisterKnownType(node.GetType());
                    service.RegisterKnownType(node.Value.GetType());
                }
            }
        }
    }
}
