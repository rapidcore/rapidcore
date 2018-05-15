using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.ReflectionBased;

namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// Simple POCO based ORM for Google Cloud Datastore
    /// </summary>
    public class DatastoreOrm
    {
        private readonly DatastoreReflector reflector;
        private readonly IEntityFactory entityFactory;
        private readonly DatastoreDb datastoreDb;
        private readonly IPocoFactory pocoFactory;

        /// <summary>
        /// Instantiate with custom stack or through Depency Injection
        /// </summary>
        /// <param name="datastoreDb">The low-level datastore connection. Needed to generate keys.</param>
        /// <param name="reflector">Answers questions about a POCO</param>
        /// <param name="entityFactory">Creates entities from POCOs</param>
        /// <param name="pocoFactory">Creates POCOs from entities</param>
        public DatastoreOrm(
            DatastoreDb datastoreDb,
            DatastoreReflector reflector,
            IEntityFactory entityFactory,
            IPocoFactory pocoFactory
        )
        {
            this.datastoreDb = datastoreDb;
            this.reflector = reflector;
            this.entityFactory = entityFactory;
            this.pocoFactory = pocoFactory;
        }

        /// <summary>
        /// Instantiate using the default stack
        /// </summary>
        /// <param name="datastoreDb">The low-level datastore connection. Needed to generate keys.</param>
        public DatastoreOrm(DatastoreDb datastoreDb)
        {
            this.datastoreDb = datastoreDb;
            reflector = new DatastoreReflector();
            entityFactory = new ReflectionBasedEntityFactory(reflector);
            pocoFactory = new ReflectionBasedPocoFactory(reflector);
        }

        /// <summary>
        /// Get the kind for the given type
        /// </summary>
        /// <param name="type">Your POCO type</param>
        /// <returns>The kind to use</returns>
        public virtual string GetKind(Type type)
        {
            return reflector.GetKind(type);
        }

        /// <summary>
        /// Get string-based key
        /// </summary>
        /// <param name="kind">The kind that the key will live in</param>
        /// <param name="id">The ID to use</param>
        public virtual Key GetKey(string kind, string id)
        {
            return datastoreDb.CreateKeyFactory(kind).CreateKey(id);
        }
        
        /// <summary>
        /// Get long-based key
        /// </summary>
        /// <param name="kind">The kind that the key will live in</param>
        /// <param name="id">The ID to use</param>
        public virtual Key GetKey(string kind, long id)
        {
            return datastoreDb.CreateKeyFactory(kind).CreateKey(id);
        }

        /// <summary>
        /// Create an entity based on the given POCO
        /// </summary>
        /// <param name="poco">The POCO</param>
        /// <returns>An entity representing the POCO</returns>
        public virtual Entity PocoToEntity(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot create an Entity from null");
            }

            var kind = GetKind(poco.GetType());
            return entityFactory.FromPoco(datastoreDb, kind, poco);
        }
        
        /// <summary>
        /// Create an entity based on the given POCO
        /// </summary>
        /// <param name="poco">The POCO</param>
        /// <param name="kind">The kind</param>
        /// <returns>An entity representing the POCO</returns>
        public virtual Entity PocoToEntity(object poco, string kind)
        {
            return entityFactory.FromPoco(datastoreDb, kind, poco);
        }

        /// <summary>
        /// Create a POCO based on the given entity
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <typeparam name="TPoco">The POCO type to create</typeparam>
        /// <returns>A fresh hydrated POCO</returns>
        public virtual TPoco EntityToPoco<TPoco>(Entity entity) where TPoco : new()
        {
            return pocoFactory.FromEntity<TPoco>(entity);
        }
    }
}