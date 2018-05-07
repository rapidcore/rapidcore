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
        
        public Entity FromPoco(string kind, object poco)
        {
            throw new System.NotImplementedException();
            
            /*
            var entity = new Entity();

            var id = GetIdValue(poco); // gets value of property flagged with Id or named something like "Id"
            entity.Key = dsDb.CreateKeyFactory(kind).CreateKey(id);

            foreach (property in poco)
            {
                if (property is not flagged as ignore)
                {
                    var tuple = ValueFactory.FromPropertyInfo(property); // returns tuple like <name, value>
                    entity.Properties.Add(tuple.name, tuple.value);
                }
            }

            return entity;
            */
        }
    }
}