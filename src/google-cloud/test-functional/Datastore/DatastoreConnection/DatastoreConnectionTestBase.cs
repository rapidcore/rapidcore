using RapidCore.GoogleCloud.Testing;

namespace functionaltests.Datastore.DatastoreConnection
{
    public abstract class DatastoreConnectionTestBase : DatastoreConnectedTestBase
    {
        protected readonly RapidCore.GoogleCloud.Datastore.DatastoreConnection connection;
        
        protected DatastoreConnectionTestBase()
        {
            connection = new RapidCore.GoogleCloud.Datastore.DatastoreConnection(GetDb());
        }
    }
}