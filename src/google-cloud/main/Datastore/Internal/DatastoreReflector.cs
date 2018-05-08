using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class DatastoreReflector
    {
        #region Kind
        public virtual string GetKind(object poco)
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
        #endregion

        #region ID
        protected List<Type> validIdTypes = new List<Type>
        {
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(string)
        };
        
        public virtual string GetIdValue(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot get ID from null");
            }

            var type = poco.GetType().GetTypeInfo();

            var idProps = type
                            .GetProperties()
                            .Where(IsIdProperty)
                            .ToList();

            if (idProps.Count == 0)
            {
                throw new PrimaryKeyException($"Could not find an id on {type.Name}");
            }

            if (idProps.Count > 1)
            {
                var propNames = string.Join(", ", idProps.Select(x => x.Name));
                throw new PrimaryKeyException($"More than 1 property on {type.Name} could be an id: {propNames}");
            }

            var idProp = idProps.First();

            if (idProp.HasAttribute(typeof(IgnoreAttribute)))
            {
                throw new PrimaryKeyException($"The id property {type.Name}.{idProp.Name} is marked with {nameof(IgnoreAttribute)}");
            }

            if (!validIdTypes.Contains(idProp.PropertyType))
            {
                var allowed = string.Join(", ", validIdTypes.Select(x => x.Name));
                throw new PrimaryKeyException($"The id property {type.Name}.{idProp.Name} has invalid type of {idProp.PropertyType.Name}. Only {allowed} are allowed.");
            }

            if (idProp.GetMethod == null)
            {
                throw new PrimaryKeyException($"The id property {type.Name}.{idProp.Name} has no getter");
            }
            
            if (idProp.GetMethod.IsStatic)
            {
                throw new PrimaryKeyException($"The id property {type.Name}.{idProp.Name} is static");
            }

            return idProp.GetValue(poco).ToString();
        }

        private bool IsIdProperty(PropertyInfo prop)
        {
            return IsPrimaryKeyName(prop.Name) || prop.HasAttribute(typeof(PrimaryKeyAttribute));
        }

        private bool IsPrimaryKeyName(string name)
        {
            var clean = name.ToLowerInvariant().Replace("_", "");

            return clean.Equals("id") || clean.Equals("identifier") || clean.Equals("primarykey");
        }
        #endregion

        #region Content properties
        public List<PropertyInfo> GetContentProperties(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot get content properties from null");
            }
            
            var type = poco.GetType().GetTypeInfo();

            return type
                .GetProperties()
                .Where(prop =>
                {
                    if (prop.HasAttribute(typeof(IgnoreAttribute)))
                    {
                        return false;
                    }

                    if (prop.GetMethod == null || prop.GetMethod.IsStatic)
                    {
                        return false;
                    }

                    if (IsIdProperty(prop))
                    {
                        return false;
                    }
                    
                    return true;
                })
                .ToList();
        }
        #endregion

        #region Value name
        public string GetValueName(PropertyInfo prop)
        {
            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop), "Cannot get value name from null");
            }
            
            if (prop.HasAttribute(typeof(NameAttribute)))
            {
                var attr = prop.GetSpecificAttribute(typeof(NameAttribute)).FirstOrDefault();

                return ((NameAttribute) attr)?.Name;
            }

            return prop.Name;
        }
        #endregion
    }
}