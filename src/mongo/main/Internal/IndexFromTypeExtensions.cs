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
            var entityAttributes = type.GetSpecificAttribute<EntityAttribute>();

            string collectionName = null;
            
            if (entityAttributes.Count > 0)
            {
                collectionName = entityAttributes.FirstOrDefault()?.CollectionName;
            }
            
            return collectionName ?? type.Name;
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
            if (fieldPrefix.Split('.').Count() > 20)
            {
                throw new InvalidOperationException($"Tree is too deep - could be a recursion. Current 'path' is {fieldPrefix}");
            }

            var definitions = new Dictionary<string, IndexDefinition>();

            type
                .GetProperties()
                .Where(prop => IsIndexCandidate(prop))
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
                    if (IsSubDocument(prop.PropertyType))
                    {
                        GetIndexDefinitionsWorker(prop.PropertyType.GetTypeInfo(), indexes, $"{fieldPrefix}{prop.Name}.");
                    }
                });

            indexes.AddRange(definitions.Values.ToList());
        }

        private static bool IsIndexCandidate(PropertyInfo prop)
        {
            if (prop.GetMethod == null || prop.GetMethod.IsStatic)
            {
                return false;
            }

            return true;
        }

        private static bool IsSubDocument(Type type)
        {
            return !(type.Namespace.Equals("System") || type.Namespace.StartsWith("System.")); // we should still allow someones stuff to have namespace SystemOfDoom
        }
    }
}