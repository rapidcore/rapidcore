using Google.Cloud.Datastore.V1;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    /// <summary>
    /// Creates <see cref="Entity"/> instances
    /// </summary>
    public interface IEntityFactory
    {
        /// <summary>
        /// Create an entity from a POCO
        /// </summary>
        /// <param name="datastoreDb">The datastore db instance that will "host" the entity</param>
        /// <param name="kind">The kind to make the key on</param>
        /// <param name="poco">The POCO</param>
        /// <returns>The entity matching the POCO</returns>
        Entity FromPoco(DatastoreDb datastoreDb, string kind, object poco);

        /// <summary>
        /// Create a sub-entity from a POCO.
        ///
        /// The only major difference, is that a sub-entity
        /// does not have a Key
        /// </summary>
        /// <param name="poco">The POCO</param>
        /// <returns>The entity matching the POCO</returns>
        Entity EmbeddedEntityFromPoco(object poco);
    }
}