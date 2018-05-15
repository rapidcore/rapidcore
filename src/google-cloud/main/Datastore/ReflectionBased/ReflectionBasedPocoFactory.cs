using System;
using Google.Cloud.Datastore.V1;
using RapidCore.GoogleCloud.Datastore.ReflectionBased.Internal;

namespace RapidCore.GoogleCloud.Datastore.ReflectionBased
{
    public class ReflectionBasedPocoFactory : IPocoFactory
    {
        private readonly DatastoreReflector reflector;

        public ReflectionBasedPocoFactory(DatastoreReflector reflector)
        {
            this.reflector = reflector;
        }

        public TPoco FromEntity<TPoco>(Entity entity) where TPoco : new()
        {
            var poco = new TPoco();

            reflector.SetIdValue(poco, entity.Key);
            
            SetValues(poco, entity);

            return poco;
        }

        public object FromEmbeddedEntity(Type tPoco, Entity entity)
        {
            var poco = Activator.CreateInstance(tPoco);

            SetValues(poco, entity);
            
            return poco;
        }
        
        private void SetValues(object poco, Entity entity)
        {
            reflector
                .GetContentProperties(poco)
                .ForEach(prop =>
                {
                    var name = reflector.GetValueName(prop);
                    var value = entity[name];
                    
                    prop.SetValue(poco, PocoValueFactory.FromEntityValue(prop, value, this));
                });
        }
    }
}