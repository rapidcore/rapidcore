using System;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.Internal;

namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// A high-level wrapper for Google Datastore using <see cref="DatastoreOrm"/>
    /// </summary>
    public class DatastoreConnection
    {
        private readonly DatastoreOrm orm;
        private readonly DatastoreDb datastoreDb;

        public DatastoreConnection(DatastoreDb datastoreDb) : this(datastoreDb, null)
        {
        }

        public DatastoreConnection(DatastoreDb datastoreDb, DatastoreOrm orm)
        {
            this.datastoreDb = datastoreDb;

            if (orm != null)
            {
                this.orm = orm;
            }
            else
            {
                var reflector = new DatastoreReflector();
                
                this.orm = new DatastoreOrm(
                    reflector,
                    new ReflectionBasedEntityFactory(reflector),
                    datastoreDb
                );
            }
        }
   

        /// <summary>
        /// The underlying db connection
        /// </summary>
        public virtual DatastoreDb DatastoreDb => datastoreDb;
        
        #region Insert
        /// <summary>
        /// Async insert
        /// </summary>
        /// <param name="kind">The kind to work on</param>
        /// <param name="poco">The POCO to insert</param>
        public virtual Task InsertAsync<TPoco>(string kind, TPoco poco)
        {
            return datastoreDb.InsertAsync(orm.PocoToEntity(poco, kind));
        }

        /// <summary>
        /// Async insert
        ///
        /// The kind will be determined automatically.
        /// </summary>
        /// <param name="poco">The POCO to insert</param>
        public virtual Task InsertAsync<TPoco>(TPoco poco)
        {
            var kind = orm.GetKind(typeof(TPoco));
            return InsertAsync(kind, poco);
        }
        #endregion

        #region Get by ID
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The numerical ID</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdAsync<TPoco>(long id)
        {
            var kind = orm.GetKind(typeof(TPoco));
            return GetByIdAsync<TPoco>(id, kind);
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The string ID</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdAsync<TPoco>(string id)
        {
            var kind = orm.GetKind(typeof(TPoco));
            return GetByIdAsync<TPoco>(id, kind);
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The numerical ID</param>
        /// <param name="kind">The kind on which to query</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdAsync<TPoco>(long id, string kind)
        {
            return GetByKeyAsync<TPoco>(orm.GetKey(kind, id));
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The string ID</param>
        /// <param name="kind">The kind on which to query</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdAsync<TPoco>(string id, string kind)
        {
            return GetByKeyAsync<TPoco>(orm.GetKey(kind, id));
        }
        #endregion

        #region Get by key
        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key">The key to look up</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual async Task<TPoco> GetByKeyAsync<TPoco>(Key key)
        {
            var entity = await datastoreDb.LookupAsync(key);

            return orm.EntityToPoco<TPoco>(entity);
        }
        #endregion
    }
}