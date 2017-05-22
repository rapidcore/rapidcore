using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.Mongo.Internal
{
    public static class IndexFromTypeExtensions
    {
        public static List<IndexDefinition> GetIndexDefinitions(this TypeInfo type)
        {
            var definitions = new Dictionary<string, IndexDefinition>();

            type
                .GetProperties()
                .Where(x => x.HasAttribute(typeof(IndexAttribute)))
                .Select(x => x)
                .ToList()
                .ForEach(prop =>
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
                                def.DocumentType = type.UnderlyingSystemType;
                                def.Collection = type.GetCollectionName();
                                def.Name = attribute.Name;

                                var lookupName = def.Name ?? prop.Name;

                                definitions.Add(lookupName, def);
                            }

                            def.Update(attribute, prop.Name);
                        });
                });

            return definitions.Values.ToList();
        }

        public static string GetCollectionName(this TypeInfo type)
        {
            return type.Name;
        }
    }
}