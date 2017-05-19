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

        public virtual string GetKeySpec()
        {
            var sb = new StringBuilder();

            Keys.OrderBy(k => k.Order).ToList().ForEach(k => sb.Append($"{k.Name} : 1"));

            return $"{{ {sb.ToString()} }}";
        }

        public virtual BsonDocument GetKeySpecAsAlalalaa()
        {
            var doc = new BsonDocument();

            Keys
                .OrderBy(k => k.Order)
                .ToList()
                .ForEach(k => {
                    doc.Add(k.Name, 1);
                });

            return doc;
        }

        // public virtual object GetKeySpecBababababa()
        // {
        //     typeof(IndexKeysDefinition).MakeGenericType(DocumentType);
        // }

        public virtual CreateIndexOptions GetOptions()
        {
            var options = new CreateIndexOptions();

            options.Background = true;
            options.Sparse = Sparse;

            return options;
        }
    }
}