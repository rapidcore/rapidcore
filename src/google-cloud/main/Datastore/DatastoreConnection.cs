using Google.Cloud.Datastore.V1;

namespace RapidCore.GoogleCloud.Datastore
{
    public class DatastoreConnection
    {
        private readonly DatastoreDb datastoreDb;

        public DatastoreConnection(DatastoreDb datastoreDb)
        {
            this.datastoreDb = datastoreDb;
        }

        /// <summary>
        /// The underlying db connection
        /// </summary>
        public virtual DatastoreDb DatastoreDb => datastoreDb;
    }
}