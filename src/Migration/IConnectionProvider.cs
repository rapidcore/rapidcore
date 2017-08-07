namespace RapidCore.Mongo.Migration
{
    /// <summary>
    /// Provides connection instances to Mongo
    /// </summary>
    public interface IConnectionProvider
    {
        /// <summary>
        /// Get the default connection
        /// </summary>
        MongoDbConnection Default();
        
        /// <summary>
        /// Get a named connection
        /// </summary>
        /// <param name="name">The name of the connection</param>
        MongoDbConnection Named(string name);
    }
}