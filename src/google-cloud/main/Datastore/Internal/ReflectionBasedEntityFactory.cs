using Google.Cloud.Datastore.V1;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class ReflectionBasedEntityFactory : IEntityFactory
    {
        private readonly DatastoreReflector reflection;

        public ReflectionBasedEntityFactory(DatastoreReflector reflection)
        {
            this.reflection = reflection;
        }
        
        public Entity FromPoco(DatastoreDb datastoreDb, string kind, object poco)
        {
            var entity = new Entity();

            SetKey(reflection.GetIdValue(poco), kind, datastoreDb, entity);

            AddProperties(entity, poco);

            return entity;
        }

        public Entity EmbeddedEntityFromPoco(object poco)
        {
            var entity = new Entity();
            
            AddProperties(entity, poco);

            return entity;
        }

        private void SetKey(string id, string kind, DatastoreDb datastoreDb, Entity entity)
        {
            if (long.TryParse(id, out var idLong))
            {
                entity.Key = datastoreDb.CreateKeyFactory(kind).CreateKey(idLong);
                return;
            }
            
            entity.Key = datastoreDb.CreateKeyFactory(kind).CreateKey(id);
        }
        
        private void AddProperties(Entity entity, object poco)
        {
            reflection
                .GetContentProperties(poco)
                .ForEach(prop =>
                {
                    var name = reflection.GetValueName(prop);
                    var value = EntityValueFactory.FromPropertyInfo(poco, prop, this);
                    entity.Properties.Add(name, value);
                });
        }
    }
}