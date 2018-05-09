using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Cloud.Datastore.V1;
using RapidCore.Reflection;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class DatastoreReflector
    {
        private class IdType
        {
            public Type Type { get; set; }
            public Func<object, string> ToIdOnEntity { get; set; } = pocoId => pocoId.ToString();
            public Func<Key, object> ToIdOnPoco { get; set; } = key => key.Path[0].Name;
        }
        
        #region Kind
        public virtual string GetKind(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            if (typeInfo.HasAttribute(typeof(KindAttribute)))
            {
                var attr = typeInfo.GetSpecificAttribute(typeof(KindAttribute)).FirstOrDefault();

                return ((KindAttribute) attr)?.Kind;
            }
            
            return type.Name;
        }
        
        public virtual string GetKind(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot get kind from null");
            }

            return GetKind(poco.GetType());
        }
        #endregion

        #region ID
        private readonly Dictionary<Type, IdType> validIdTypes = new Dictionary<Type, IdType>
        {
            {
                typeof(short),
                new IdType
                {
                    Type = typeof(short),
                    ToIdOnPoco = key => short.Parse(key.Path[0].Id.ToString())
                }
            },
            {
                typeof(int),
                new IdType
                {
                    Type = typeof(int),
                    ToIdOnPoco = key => int.Parse(key.Path[0].Id.ToString())
                }
            },
            {
                typeof(long),
                new IdType
                {
                    Type = typeof(long),
                    ToIdOnPoco = key => key.Path[0].Id
                }
            },
            {
                typeof(string),
                new IdType
                {
                    Type = typeof(string),
                    ToIdOnPoco = key => key.Path[0].Name
                }
            },
            {
                typeof(Guid),
                new IdType
                {
                    Type = typeof(Guid),
                    ToIdOnPoco = key => Guid.Parse(key.Path[0].Name)
                }
            }
        }; 

        public virtual PropertyInfo GetIdProperty(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            var idProps = typeInfo
                .GetProperties()
                .Where(IsIdProperty)
                .ToList();

            if (idProps.Count == 0)
            {
                throw new PrimaryKeyException($"Could not find an id on {typeInfo.Name}");
            }

            if (idProps.Count > 1)
            {
                var propNames = string.Join(", ", idProps.Select(x => x.Name));
                throw new PrimaryKeyException($"More than 1 property on {typeInfo.Name} could be an id: {propNames}");
            }

            var idProp = idProps.First();

            if (idProp.HasAttribute(typeof(IgnoreAttribute)))
            {
                throw new PrimaryKeyException($"The id property {typeInfo.Name}.{idProp.Name} is marked with {nameof(IgnoreAttribute)}");
            }

            if (!validIdTypes.ContainsKey(idProp.PropertyType))
            {
                var allowed = string.Join(", ", validIdTypes.Select(kvp => kvp.Key.Name));
                throw new PrimaryKeyException($"The id property {typeInfo.Name}.{idProp.Name} has invalid type of {idProp.PropertyType.Name}. Only {allowed} are allowed.");
            }

            if (idProp.GetMethod == null)
            {
                throw new PrimaryKeyException($"The id property {typeInfo.Name}.{idProp.Name} has no getter");
            }
            
            if (idProp.GetMethod.IsStatic)
            {
                throw new PrimaryKeyException($"The id property {typeInfo.Name}.{idProp.Name} is static");
            }

            return idProp;
        }
        
        public virtual string GetIdValue(object poco)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot get ID from null");
            }

            var idProp = GetIdProperty(poco.GetType());
            var mapper = validIdTypes[idProp.PropertyType];

            return mapper.ToIdOnEntity(idProp.GetValue(poco));
        }

        public virtual void SetIdValue(object poco, Key key)
        {
            if (poco == null)
            {
                throw new ArgumentNullException(nameof(poco), "Cannot set ID on null");
            }

            var idProp = GetIdProperty(poco.GetType());
            var mapper = validIdTypes[idProp.PropertyType];
            
            idProp.SetValue(poco, mapper.ToIdOnPoco(key));
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