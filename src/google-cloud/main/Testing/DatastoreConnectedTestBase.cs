using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;
using Grpc.Core;
using RapidCore.Environment;

namespace RapidCore.GoogleCloud.Testing
{
    /// <summary>
    /// Base class for functional tests that need access to
    /// a Google Datastore.
    /// 
    /// It provides simple helpers that we use ourselves.
    /// </summary>
    public abstract class DatastoreConnectedTestBase
    {
        private DatastoreDb db;
        private bool isConnected = false;

        protected virtual string GetProjectId()
        {
            var envVars = new EnvironmentVariables();
            return envVars.Get("RAPIDCORE_DATASTORE_PROJECT_ID", "rapidcore-local");
        }
        
        protected virtual string GetConnectionString()
        {
            var envVars = new EnvironmentVariables();
            return envVars.Get("RAPIDCORE_DATASTORE_URL", "localhost:8081");
        }
        
        protected virtual string GetNamespace()
        {
            return GetType().Name;
        }

        protected virtual void Connect()
        {
            if (!isConnected)
            {
                isConnected = true;
                db = DatastoreDb.Create(GetProjectId(), GetNamespace(), GetClient());
                
                // truncate the namespace ... aka remove all kinds
                var query = new Query("__kind__") { Limit = int.MaxValue };

                foreach (var kindEntity in db.RunQuery(query).Entities)
                {
                    EnsureEmptyKind(kindEntity.Key.Path[0].Name);
                }
            }
        }

        private Uri ToUri(string url)
        {
            var uri = new Uri(url);

            if (string.IsNullOrEmpty(uri.Host))
            {
                return new Uri($"fake://{url}");
            }

            return uri;
        }

        protected virtual DatastoreClient GetClient()
        {
            var url = ToUri(GetConnectionString());
            
            return DatastoreClient.Create(
                new Channel(url.Host, url.Port, ChannelCredentials.Insecure),
                DatastoreSettings.GetDefault()
            );
        }

        protected virtual DatastoreDb GetDb()
        {
            Connect();
            return db;
        }

        protected void EnsureEmptyKind(string kind)
        {
            var query = new Query(kind)
            {
                Limit = int.MaxValue,
                Projection = {new Projection("__key__")}
            };

            var res = GetDb().RunQuery(query);
            
            GetDb().Delete(res.Entities.Select(x => x.Key));
        }

        protected virtual void Insert(Entity entity)
        {
            GetDb().Insert(entity);
        }

        protected virtual IList<Entity> GetAll(string kind)
        {
            var query = new Query(kind)
            {
                Limit = int.MaxValue
            };
            
            var queryRes = GetDb().RunQuery(query);
            return new List<Entity>(queryRes.Entities);
        }

        protected virtual Key GetKey(string kind, string id)
        {
            return GetDb().CreateKeyFactory(kind).CreateKey(id);
        }

        protected virtual Key GetKey(string kind, long id)
        {
            return GetDb().CreateKeyFactory(kind).CreateKey(id);
        }
        
        /// <summary>
        /// This is a work-around for the following issue:
        /// https://issuetracker.google.com/issues/111182297
        ///
        /// It simply repeats the test a few times if it fails.
        /// If it continues to fail, the resulting exception is rethrown.
        ///
        /// Hopefully this method will no longer be needed in the near future!
        /// </summary>
        /// <param name="theTestAsync">The test code to repeat</param>
        protected async Task WorkAroundDatastoreEmulatorIssueAsync(Func<Task> theTestAsync)
        {
            var i = 0;
            while (true)
            {
                i++;
                try
                {
                    await theTestAsync.Invoke();
                    break;
                }
                catch (Exception)
                {
                    if (i == 10)
                    {
                        throw;
                    }
                }
            }
        }
    }
}