using System.Collections.Generic;

namespace RapidCore.Mongo.Migration
{
    public class ConnectionProvider : IConnectionProvider
    {
        protected readonly IDictionary<string, MongoDbConnection> connections = new Dictionary<string, MongoDbConnection>();
        
        public MongoDbConnection Default()
        {
            return Named("UseAsDefault");
        }

        public MongoDbConnection Named(string name)
        {
            return connections[name.ToLower()];
        }

        public void Add(string name, MongoDbConnection connection, bool useAsDefault)
        {
            connections.Add(name.ToLower(), connection);
            if (useAsDefault)
            {
                connections.Add("UseAsDefault".ToLower(), connection);
            }
        }
    }
}