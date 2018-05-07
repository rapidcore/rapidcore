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
        /// <param name="kind">The kind to make the key on</param>
        /// <param name="poco">The POCO</param>
        /// <returns>The entity matching the POCO</returns>
        Entity FromPoco(string kind, object poco);
    }
}