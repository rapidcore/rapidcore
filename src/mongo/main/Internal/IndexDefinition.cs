using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace RapidCore.Mongo.Internal
{
    /// <summary>
    /// Defines an index
    /// </summary>
    public class IndexDefinition
    {
        /// <summary>
        /// The type of the document
        /// </summary>
        public virtual Type DocumentType { get; set; }

        /// <summary>
        /// The name of the collection
        /// </summary>
        public virtual string Collection { get; set; }

        /// <summary>
        /// The name of the index
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Whether or not to make the index sparse
        /// </summary>
        public virtual bool Sparse { get; set; } = false;

        /// <summary>
        /// Whether to make a unique index
        /// </summary>
        public virtual bool Unique { get; set; } = false;

        /// <summary>
        /// The keys of the index
        /// </summary>
        public virtual IList<IndexKey> Keys { get; set; } = new List<IndexKey>();

        /// <summary>
        /// Update this index with the given attribute
        /// </summary>
        /// <param name="attribute">The index attribute instance</param>
        /// <param name="field">The field having the attribute</param>
        public virtual void Update(IndexAttribute attribute, string field)
        {
            if (attribute.Sparse)
            {
                Sparse = true;
            }

            if (attribute.Unique)
            {
                Unique = true;
            }

            Keys.Add(new IndexKey { Name = field, Order = attribute.Order });
        }

        /// <summary>
        /// Generates an IndexKeysDefinition instance for this index
        /// </summary>
        /// <returns><c>BsonDocumentIndexKeysDefinition&lt;DocumentType&gt;</c></returns>
        public virtual object GetKeySpec()
        {
            var indexDefinitionsType = typeof(BsonDocumentIndexKeysDefinition<>).MakeGenericType(DocumentType);
            return Activator.CreateInstance(indexDefinitionsType, GetKeySpecBsonDocument());
        }

        /// <summary>
        /// Get options for creating the index
        /// </summary>
        public virtual CreateIndexOptions GetOptions()
        {
            var options = new CreateIndexOptions
            {
                Background = true,
                Sparse = Sparse,
                Name = Name,
                Unique = Unique,
            };

            return options;
        }

        /// <summary>
        /// Generate a <see cref="BsonDocument" /> specifying
        /// the keys of the index
        /// </summary>
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