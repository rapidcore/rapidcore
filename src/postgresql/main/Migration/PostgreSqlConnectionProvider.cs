using System.Collections.Generic;
using System.Data;

namespace RapidCore.PostgreSql.Migration
{
    public class PostgreSqlConnectionProvider
    {
        protected readonly IDictionary<string, IDbConnection> connections = new Dictionary<string, IDbConnection>();

        public IDbConnection Default()
        {
            return Named("UseAsDefault");
        }

        public virtual IDbConnection Named(string name)
        {
            return connections[name.ToLowerInvariant()];
        }
        public virtual void Add(string name, IDbConnection connection, bool useAsDefault)
        {
            connections.Add(name.ToLowerInvariant(), connection);
            if (useAsDefault)
            {
                connections.Add("UseAsDefault".ToLowerInvariant(), connection);
            }
        }


    }
}