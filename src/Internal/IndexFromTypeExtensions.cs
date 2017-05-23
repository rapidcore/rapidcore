using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.Mongo.Internal
{
    public static class IndexFromTypeExtensions
    {
        public static string GetCollectionName(this TypeInfo type)
        {
            return type.Name;
        }

        public static IndexDefinitionCollection GetIndexDefinitions(this TypeInfo type)
        {
            var definitions = new IndexDefinitionCollection();

            GetIndexDefinitionsWorker(type, definitions, string.Empty);

            definitions
                .SetDocumentType(type.UnderlyingSystemType)
                .SetCollectionName(type.GetCollectionName());

            return definitions;
        }

        private static void GetIndexDefinitionsWorker(TypeInfo type, IndexDefinitionCollection indexes, string fieldPrefix)
        {
            var definitions = new Dictionary<string, IndexDefinition>();

            type
                .GetProperties()
                .ToList()
                .ForEach(prop =>
                {
                    if (prop.HasAttribute(typeof(IndexAttribute)))
                    {
                        prop
                            .GetSpecificAttribute(typeof(IndexAttribute))
                            .ForEach(a =>
                            {
                                var attribute = (IndexAttribute)a;
                                IndexDefinition def;

                                if (!string.IsNullOrEmpty(attribute.Name) && definitions.ContainsKey(attribute.Name))
                                {
                                    def = definitions[attribute.Name];
                                }
                                else
                                {
                                    def = new IndexDefinition();
                                    def.Name = attribute.Name;

                                    var lookupName = def.Name ?? prop.Name;

                                    definitions.Add(lookupName, def);
                                }

                                def.Update(attribute, $"{fieldPrefix}{prop.Name}");
                            });
                    }

                    // if the type of the property could have its own
                    // properties with [Index], we should look at those too
                    if (!prop.PropertyType.Namespace.StartsWith("System."))
                    {
                        GetIndexDefinitionsWorker(prop.PropertyType.GetTypeInfo(), indexes, $"{fieldPrefix}{prop.Name}.");
                    }
                });

            indexes.AddRange(definitions.Values.ToList());
        }
    }
}