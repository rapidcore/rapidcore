using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Rapidcore.Postgresql
{
    public class PostgreSqlConnectionProvider
    {
        protected readonly IDictionary<string, IDbConnection> connections = new Dictionary<string, IDbConnection>();

        public IDbConnection Default()
        {
            return Named("UseAsDefault");
        }

        public IDbConnection Named(string name)
        {
            return connections[name.ToLowerInvariant()];
        }
        public void Add(string name, IDbConnection connection, bool useAsDefault)
        {
            connections.Add(name.ToLowerInvariant(), connection);
            if (useAsDefault)
            {
                connections.Add("UseAsDefault".ToLowerInvariant(), connection);
            }
        }


    }
}