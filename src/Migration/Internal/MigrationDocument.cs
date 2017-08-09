using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RapidCore.Mongo.Migration.Internal
{
    /// <summary>
    /// Mongodb document for storing migrations that have been executed
    /// </summary>
    public class MigrationDocument
    {
        [BsonIgnore] public static string CollectionName = "__RapidCoreMigrations";

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
        public List<string> StepsCompleted { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether [migration completed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [migration completed]; otherwise, <c>false</c>.
        /// </value>
        public bool MigrationCompleted { get; set; }

        /// <summary>
        /// Gets or sets the total migration time in ms.
        /// </summary>
        /// <value>
        /// The total migration time in ms.
        /// </value>
        public long TotalMigrationTimeInMs { get; set; }

        /// <summary>
        /// Gets or sets the completed at date and time in UTC.
        /// </summary>
        /// <value>
        /// The completed at.
        /// </value>
        public DateTime CompletedAt { get; set; }
    }
}
