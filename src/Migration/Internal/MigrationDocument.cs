using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RapidCore.Mongo.Migration.Internal
{
    /// <summary>
    /// Mongodb document for storing migrations that have been executed
    /// </summary>
    public class MigrationDocument
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the migration.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Index(Unique = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the list of steps that have been successfully completed.
        /// </summary>
        /// <value>
        /// The steps completed.
        /// </value>
        public List<string> StepsCompleted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [migration completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [migration completed]; otherwise, <c>false</c>.
        /// </value>
        public bool MigrationCompleted { get; set; }
    }
}
