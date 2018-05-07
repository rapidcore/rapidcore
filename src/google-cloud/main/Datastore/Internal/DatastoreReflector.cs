using System;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class DatastoreReflector
    {
        public string GetKind(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot get kind from null");
            }
            
            var type = poco.GetType().GetTypeInfo();

            if (type.HasAttribute(typeof(KindAttribute)))
            {
                var attr = type.GetSpecificAttribute(typeof(KindAttribute)).FirstOrDefault();

                return ((KindAttribute) attr)?.Kind;
            }
            
            return poco.GetType().Name;
        }
    }
}