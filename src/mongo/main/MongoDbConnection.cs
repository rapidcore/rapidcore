using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MongoDB.Driver;
using RapidCore.Mongo.Internal;

namespace RapidCore.Mongo
{
    /// <summary>
    /// High-level connection to a MongoDB
    /// </summary>
    public class MongoDbConnection
    {
        private readonly IMongoDatabase mongoDb;

        public MongoDbConnection(IMongoDatabase mongoDb)
        {
            this.mongoDb = mongoDb;
        }
        
        /// <summary>
        /// Get the underlying database
        /// </summary>
        public virtual IMongoDatabase Database => mongoDb;
        
        /// <summary>
        /// Get the underlying client
        /// </summary>
        public virtual IMongoClient Client => mongoDb.Client;

        /// <summary>
        /// Straight up first or default with filter
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>The document or default(TDocument)</returns>
        public virtual Task<TDocument> FirstOrDefaultAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            return FirstOrDefaultAsync(typeof(TDocument).GetTypeInfo().GetCollectionName(), filter);
        }
        
        /// <summary>
        /// Straight up first or default with filter
        /// </summary>
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="filter">Filter expression</param>
        /// <returns>The document or default(TDocument)</returns>
        public virtual Task<TDocument> FirstOrDefaultAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName)
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async insert
        /// </summary>
        /// <param name="doc">The document to insert</param>
        public virtual Task InsertAsync<TDocument>(TDocument doc)
        {
            return InsertAsync<TDocument>(typeof(TDocument).GetTypeInfo().GetCollectionName(), doc);
        }

        /// <summary>
        /// Async insert
        /// </summary>
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="doc">The document to insert</param>
        public virtual Task InsertAsync<TDocument>(string collectionName, TDocument doc)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName)
                .InsertOneAsync(doc);
        }

        /// <summary>
        /// Async upsert
        /// </summary>
        /// <param name="doc">The document to upsert</param>
        /// <param name="filter">Filter for finding the document to replace</param>
        public virtual Task UpsertAsync<TDocument>(TDocument doc, Expression<Func<TDocument, bool>> filter)
        {
            return UpsertAsync<TDocument>(typeof(TDocument).GetTypeInfo().GetCollectionName(), doc, filter);
        }

        /// <summary>
        /// Async upsert
        /// </summary>
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="doc">The document to upsert</param>
        /// <param name="filter">Filter for finding the document to replace</param>
        public virtual Task UpsertAsync<TDocument>(string collectionName, TDocument doc, Expression<Func<TDocument, bool>> filter)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName)
                .ReplaceOneAsync(filter, doc, new UpdateOptions { IsUpsert = true });
        }

        /// <summary>
        /// Run an async delete query
        /// </summary>
        /// <param name="filter">Filter for finding the documents to delete</param>
        public virtual Task DeleteAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
        {
            return DeleteAsync(typeof(TDocument).GetTypeInfo().GetCollectionName(), filter);
        }

        /// <summary>
        /// Run an async delete query
        /// </summary>
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="filter">Filter for finding the documents to delete</param>
        public virtual Task DeleteAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName)
                .DeleteManyAsync(filter);
        }

        /// <summary>
        /// UNSTABLE API!!
        /// 
        /// Get all documents that match the given filter.
        /// </summary>
        /// <param name="filter">Filter for finding documents</param>
        /// <param name="limit">Optional limit on how many documents you want</param>
        public virtual async Task<IList<TDocument>> GetAsync<TDocument>(Expression<Func<TDocument, bool>> filter, int? limit = null)
        {
            return await GetAsync<TDocument>(typeof(TDocument).GetTypeInfo().GetCollectionName(), filter, limit);
        }

        /// <summary>
        /// UNSTABLE API!!
        /// 
        /// Get all documents that match the given filter.
        /// </summary>
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="filter">Filter for finding documents</param>
        /// <param name="limit">Optional limit on how many documents you want</param>
        public virtual async Task<IList<TDocument>> GetAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter, int? limit = null)
        {
            var options = new FindOptions<TDocument, TDocument>();
            if (limit.HasValue)
            {
                options.Limit = limit.Value;
            }
            
            return (await this.mongoDb
                    .GetCollection<TDocument>(collectionName)
                    .FindAsync<TDocument>(filter, options))
                    .ToList();
        }

        /// <summary>
        /// UNSTABLE API!!
        /// 
        /// Get an <see cref="IMongoCollection{TDocument}" /> to work on. This
        /// is to enable consumers to do advanced stuff that requires more
        /// freedom than we can provide.
        /// 
        /// This method does however provide a Mocking "hook-point".
        /// </summary>
        public virtual IMongoCollection<TDocument> GetCollection<TDocument>()
        {
            return GetCollection<TDocument>(typeof(TDocument).GetTypeInfo().GetCollectionName());
        }

        /// <summary>
        /// UNSTABLE API!!
        /// 
        /// Get an <see cref="IMongoCollection{TDocument}" /> to work on. This
        /// is to enable consumers to do advanced stuff that requires more
        /// freedom than we can provide.
        /// 
        /// This method does however provide a Mocking "hook-point".
        /// </summary>
        /// <param name="collectionName">The name of the collection. Defaults to the collection name given by <typeparamref name="TDocument" /></param>
        public virtual IMongoCollection<TDocument> GetCollection<TDocument>(string collectionName)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName ?? typeof(TDocument).GetTypeInfo().GetCollectionName());
        }

        /// <summary>
        /// Convert IAsyncCursor to a MongoAsyncCursor
        /// This can be used to improve testabilty as MongoAsyncCursor is a mockable wrapper for the IAsyncCursor
        /// </summary>
        /// <param name="cursor">The IAsyncCursor to convert</param>
        /// <typeparam name="TDocument">The type of the document associated with the <paramref name="cursor"/></typeparam>
        /// <returns>An instance of MongoAsyncCursor wrapping around <paramref name="cursor"/></returns>
        public virtual MongoAsyncCursor<TDocument> ConvertToMongoAsyncCursor<TDocument>(IAsyncCursor<TDocument> cursor)
        {
            return new MongoAsyncCursor<TDocument>(cursor);
        }
    }
}