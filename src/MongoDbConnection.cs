using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

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
        /// <param name="collectionName">The collection to work on</param>
        /// <param name="filter">Filter for finding the documents to delete</param>
        public virtual Task DeleteAsync<TDocument>(string collectionName, Expression<Func<TDocument, bool>> filter)
        {
            return this.mongoDb
                .GetCollection<TDocument>(collectionName)
                .DeleteManyAsync(filter);
        }
    }
}