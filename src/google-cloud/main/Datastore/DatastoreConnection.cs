using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.Internal;

namespace RapidCore.GoogleCloud.Datastore
{
    public class DatastoreConnection
    {
        private readonly DatastoreDb datastoreDb;
        private readonly IEntityFactory entityFactory;

        public DatastoreConnection(DatastoreDb datastoreDb) : this(datastoreDb, null)
        {
        }
        
        public DatastoreConnection(DatastoreDb datastoreDb, IEntityFactory entityFactory)
        {
            this.datastoreDb = datastoreDb;

            if (entityFactory != null)
            {
                this.entityFactory = entityFactory;
            }
            else
            {
                this.entityFactory = new ReflectionBasedEntityFactory(
                    new DatastoreReflector()
                ); 
            }
        }
   

        /// <summary>
        /// The underlying db connection
        /// </summary>
        public virtual DatastoreDb DatastoreDb => datastoreDb;
        
        /// <summary>
        /// Async insert
        /// </summary>
        /// <param name="kind">The kind to work on</param>
        /// <param name="poco">The POCO to insert</param>
        public virtual Task InsertAsync(string kind, object poco)
        {
            return datastoreDb.InsertAsync(entityFactory.FromPoco(datastoreDb, kind, poco));
        }

        public virtual Task InsertAsync(object poco)
        {
            return datastoreDb.InsertAsync(entityFactory.FromPoco(datastoreDb, poco));
        }
    }
}