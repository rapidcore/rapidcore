using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RapidCore.Mongo.Internal
{
    public class IndexDefinition
    {
        public virtual Type DocumentType { get; set; }
        public virtual string Collection { get; set; }
        public virtual string Name { get; set; }
        public virtual bool Sparse { get; set; } = false;
        public virtual IList<IndexKey> Keys { get; set; } = new List<IndexKey>();

        public virtual void Update(IndexAttribute attribute, string field)
        {
            if (attribute.Sparse)
            {
                Sparse = true;
            }

            Keys.Add(new IndexKey { Name = field, Order = attribute.Order });
        }

        /// <summary>
        /// Generates an IndexKeysDefinition instance for this index
        /// </summary>
        /// <returns>BsonDocumentIndexKeysDefinition<DocumentType></returns>
        public virtual object GetKeySpec()
        {
            var indexDefinitionsType = typeof(BsonDocumentIndexKeysDefinition<>).MakeGenericType(DocumentType);
            return Activator.CreateInstance(indexDefinitionsType, GetKeySpecBsonDocument());
        }

        public virtual CreateIndexOptions GetOptions()
        {
            var options = new CreateIndexOptions();

            options.Background = true;
            options.Sparse = Sparse;
            options.Name = Name;

            return options;
        }

        private BsonDocument GetKeySpecBsonDocument()
        {
            var doc = new BsonDocument();

            Keys
                .OrderBy(k => k.Order)
                .ToList()
                .ForEach(k =>
                {
                    doc.Add(k.Name, 1);
                });

            return doc;
        }
    }
}