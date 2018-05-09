using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.Internal;

namespace RapidCore.GoogleCloud.Datastore
{
    public class DatastoreOrm
    {
        private readonly DatastoreReflector reflector;
        private readonly IEntityFactory entityFactory;
        private readonly DatastoreDb datastoreDb;
        private readonly IPocoFactory pocoFactory;

        public DatastoreOrm(
            DatastoreReflector reflector,
            IEntityFactory entityFactory,
            DatastoreDb datastoreDb,
            IPocoFactory pocoFactory
        )
        {
            this.reflector = reflector;
            this.entityFactory = entityFactory;
            this.datastoreDb = datastoreDb;
            this.pocoFactory = pocoFactory;
        }

        public virtual string GetKind(Type type)
        {
            return reflector.GetKind(type);
        }

        public virtual Key GetKey(string kind, string id)
        {
            return datastoreDb.CreateKeyFactory(kind).CreateKey(id);
        }
        
        public virtual Key GetKey(string kind, long id)
        {
            return datastoreDb.CreateKeyFactory(kind).CreateKey(id);
        }

        public virtual Entity PocoToEntity(object poco, string kind)
        {
            return entityFactory.FromPoco(datastoreDb, kind, poco);
        }

        public virtual TPoco EntityToPoco<TPoco>(Entity entity) where TPoco : new()
        {
            return pocoFactory.FromEntity<TPoco>(entity);
        }
    }
}