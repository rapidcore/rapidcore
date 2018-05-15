using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.ReflectionBased.Internal;

namespace RapidCore.GoogleCloud.Datastore.ReflectionBased
{
    public class ReflectionBasedEntityFactory : IEntityFactory
    {
        private readonly DatastoreReflector reflection;

        public ReflectionBasedEntityFactory(DatastoreReflector reflection)
        {
            this.reflection = reflection;
        }

        public Entity FromPoco(DatastoreDb datastoreDb, object poco)
        {
            return FromPoco(datastoreDb, reflection.GetKind(poco), poco);
        }
        
        public Entity FromPoco(DatastoreDb datastoreDb, string kind, object poco)
        {
            var entity = new Entity();

            SetKey(reflection.GetIdValue(poco), kind, datastoreDb, entity);

            AddProperties(entity, poco, new List<string>());

            return entity;
        }

        public Entity EmbeddedEntityFromPoco(object poco, IList<string> recursionPath)
        {
            if (recursionPath.Count >= DatastoreConstantsRapidCore.MaxRecursionDepth)
            {
                var path = string.Join(".", recursionPath);
                throw new RecursionException($"Recursion depth has reached {recursionPath.Count} - bailing out.{System.Environment.NewLine}Path: {path}");
            }
            
            var entity = new Entity();
            
            AddProperties(entity, poco, recursionPath);

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
        
        private void AddProperties(Entity entity, object poco, IList<string> recursionPath)
        {
            reflection
                .GetContentProperties(poco)
                .ForEach(prop =>
                {
                    var name = reflection.GetValueName(prop);
                    var value = EntityValueFactory.FromPropertyInfo(poco, prop, this, recursionPath);
                    entity.Properties.Add(name, value);
                    recursionPath.Clear();
                });
        }
    }
}