using System;
using Google.Cloud.Datastore.V1;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    /// <summary>
    /// Converts from <see cref="Entity"/> to POCO
    /// </summary>
    public interface IPocoFactory
    {
        /// <summary>
        /// Convert a given entity to a POCO
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <typeparam name="TPoco">The type of POCO to generate</typeparam>
        TPoco FromEntity<TPoco>(Entity entity) where TPoco : new();

        object FromEmbeddedEntity(Type tPoco, Entity entity);
    }
}