﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.ReflectionBased;

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
                    datastoreDb,
                    reflector,
                    new ReflectionBasedEntityFactory(reflector),
                    new ReflectionBasedPocoFactory(reflector)
                );
            }
        }
   

        /// <summary>
        /// The underlying db connection
        /// </summary>
        public virtual DatastoreDb DatastoreDb => datastoreDb;

        /// <summary>
        /// The underlying ORM
        /// </summary>
        public virtual DatastoreOrm Orm => orm;

        /// <summary>
        /// Get the kind of a POCO
        ///
        /// This is meant as a convenience for consumers who might be
        /// building GQL queries or otherwise needs the kind of a POCO.
        /// The same thing could be achieved by using <see cref="DatastoreOrm"/> directly.
        /// </summary>
        /// <typeparam name="TPoco">The POCO you wish to get the kind for</typeparam>
        public virtual string GetKind<TPoco>()
        {
            return orm.GetKind(typeof(TPoco));
        }
        
        #region Insert
        /// <summary>
        /// Async insert
        /// </summary>
        /// <param name="poco">The POCO to insert</param>
        /// <param name="kind">The kind to work on</param>
        public virtual Task InsertAsync<TPoco>(TPoco poco, string kind)
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
            return InsertAsync(poco, kind);
        }
        #endregion

        #region Upsert
        public virtual Task UpsertAsync<TPoco>(TPoco poco) where TPoco : new()
        {
            return UpsertAsync(poco, orm.GetKind(poco.GetType()));
        }
        
        public virtual Task UpsertAsync<TPoco>(TPoco poco, string kind) where TPoco : new()
        {
            return datastoreDb.UpsertAsync(orm.PocoToEntity(poco, kind));
        }
        #endregion
        
        #region Update
        public virtual Task UpdateAsync<TPoco>(TPoco poco) where TPoco : new()
        {
            return UpdateAsync(poco, orm.GetKind(poco.GetType()));
        }
        
        public virtual Task UpdateAsync<TPoco>(TPoco poco, string kind) where TPoco : new()
        {
            return datastoreDb.UpdateAsync(orm.PocoToEntity(poco, kind));
        }
        #endregion

        #region Get by ID
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The numerical ID</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdOrDefaultAsync<TPoco>(long id) where TPoco : new()
        {
            var kind = orm.GetKind(typeof(TPoco));
            return GetByIdOrDefaultAsync<TPoco>(id, kind);
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The string ID</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdOrDefaultAsync<TPoco>(string id) where TPoco : new()
        {
            var kind = orm.GetKind(typeof(TPoco));
            return GetByIdOrDefaultAsync<TPoco>(id, kind);
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The numerical ID</param>
        /// <param name="kind">The kind on which to query</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdOrDefaultAsync<TPoco>(long id, string kind) where TPoco : new()
        {
            return GetByKeyOrDefaultAsync<TPoco>(orm.GetKey(kind, id));
        }
        
        /// <summary>
        /// Get by ID
        /// </summary>
        /// <param name="id">The string ID</param>
        /// <param name="kind">The kind on which to query</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual Task<TPoco> GetByIdOrDefaultAsync<TPoco>(string id, string kind) where TPoco : new()
        {
            return GetByKeyOrDefaultAsync<TPoco>(orm.GetKey(kind, id));
        }
        #endregion

        #region Get by key
        /// <summary>
        /// Get by key
        /// </summary>
        /// <param name="key">The key to look up</param>
        /// <typeparam name="TPoco">The type of class you want to get back</typeparam>
        /// <returns>A hydrated instance of <typeparamref name="TPoco"/> or <c>null</c></returns>
        public virtual async Task<TPoco> GetByKeyOrDefaultAsync<TPoco>(Key key) where TPoco : new()
        {
            var entity = await datastoreDb.LookupAsync(key);

            if (entity != null)
            {
                return orm.EntityToPoco<TPoco>(entity);
            }

            return default(TPoco);
        }
        #endregion

        #region Delete
        public virtual Task DeleteAsync<TPoco>(long id) where TPoco : new()
        {
            var kind = orm.GetKind(typeof(TPoco));
            return DeleteAsync(id, kind);
        }
        
        public virtual Task DeleteAsync<TPoco>(string id) where TPoco : new()
        {
            var kind = orm.GetKind(typeof(TPoco));
            return DeleteAsync(id, kind);
        }

        public virtual Task DeleteAsync(long id, string kind)
        {
            return DeleteAsync(orm.GetKey(kind, id));
        }
        
        public virtual Task DeleteAsync(string id, string kind)
        {
            return DeleteAsync(orm.GetKey(kind, id));
        }

        public virtual Task DeleteAsync(Key key)
        {
            return datastoreDb.DeleteAsync(key);
        }
        #endregion

        #region Query
        /// <summary>
        /// Get a list of POCOs from a query
        ///
        /// You can ignore the kind, as we look it up with the ORM anyway.
        /// </summary>
        /// <param name="query">The query to run (you can ignore the kind)</param>
        /// <typeparam name="TPoco">The POCO you want back</typeparam>
        public virtual Task<IList<TPoco>> Query<TPoco>(Query query) where TPoco : new()
        {
            if (query.Kind.Count == 0)
            {
                return Query<TPoco>(query, orm.GetKind(typeof(TPoco)));
            }
            
            return Query<TPoco>(query, "ignored");
        }
        
        /// <summary>
        /// Get a list of POCOs from a query
        /// </summary>
        /// <param name="query">The query to run (you can ignore the kind)</param>
        /// <param name="kind">The kind to use</param>
        /// <typeparam name="TPoco">The POCO you want back</typeparam>
        public virtual async Task<IList<TPoco>> Query<TPoco>(Query query, string kind) where TPoco : new()
        {
            var q = query;

            if (query.Kind.Count == 0)
            {
                q = new Query(kind);
                q.MergeFrom(query);
            }

            var res = await datastoreDb.RunQueryAsync(q);

            return res.Entities.Select(x => orm.EntityToPoco<TPoco>(x)).ToList();
        }
        
        /// <summary>
        /// Get a list of POCOs from a query using GQL
        /// </summary>
        /// <param name="query">The query to run (you can ignore the kind)</param>
        /// <typeparam name="TPoco">The POCO you want back</typeparam>
        public virtual async Task<IList<TPoco>> Query<TPoco>(GqlQuery query) where TPoco : new()
        {
            var res = await datastoreDb.RunQueryAsync(query);

            return res.Entities.Select(x => orm.EntityToPoco<TPoco>(x)).ToList();
        }
        #endregion
    }
}