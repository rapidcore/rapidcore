using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RapidCore.Mongo.Internal
{
    public class IndexDefinitionCollection : Collection<IndexDefinition>
    {
        public IndexDefinitionCollection()
        : base()
        {
        }

        public bool IsEmpty => this.Count == 0;

        public IndexDefinition First => this.Items[0];

        public IndexDefinitionCollection AddRange(IList<IndexDefinition> otherCollection)
        {
            foreach (var index in otherCollection)
            {
                if (!IsEmpty)
                {
                    index.Collection = First.Collection;
                    index.DocumentType = First.DocumentType;
                }

                Add(index);
            }

            return this;
        }

        public IndexDefinitionCollection SetCollectionName(string collectionName)
        {
            foreach (var index in Items)
            {
                index.Collection = collectionName;
            }

            return this;
        }

        public IndexDefinitionCollection SetDocumentType(Type documentType)
        {
            foreach (var index in Items)
            {
                index.DocumentType = documentType;
            }

            return this;
        }
    }
}