using System;
using System.Collections.Generic;
using System.Linq;
using Google.Cloud.Datastore.V1;
using Grpc.Core;

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

        protected string ProjectId { get; set; } = "rapidcore-local";
        protected string ConnectionString { get; set; } = "http://localhost:8081";

        protected string GetNamespace()
        {
            return GetType().Name;
        }

        protected void Connect()
        {
            if (!isConnected)
            {
                isConnected = true;
                db = DatastoreDb.Create(ProjectId, GetNamespace(), GetClient());
                
                // truncate the namespace ... aka remove all kinds
                var query = new Query("__kind__") { Limit = int.MaxValue };

                foreach (var kindEntity in db.RunQuery(query).Entities)
                {
                    EnsureEmptyKind(kindEntity.Key.Path[0].Name);
                }
            }
        }

        protected virtual DatastoreClient GetClient()
        {
            var url = new Uri(ConnectionString);
            
            return DatastoreClient.Create(
                new Channel(url.Host, url.Port, ChannelCredentials.Insecure),
                DatastoreSettings.GetDefault()
            );
        }

        protected DatastoreDb GetDb()
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

        protected void Insert(Entity entity)
        {
            GetDb().Insert(entity);
        }

        protected IList<Entity> GetAll(string kind)
        {
            var query = new Query(kind)
            {
                Limit = int.MaxValue
            };
            
            var queryRes = GetDb().RunQuery(query);
            return new List<Entity>(queryRes.Entities);
        }
    }
}