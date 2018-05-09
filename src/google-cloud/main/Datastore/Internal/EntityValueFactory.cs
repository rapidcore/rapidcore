using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Google.Cloud.Datastore.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using RapidCore.Reflection;
using Type = System.Type;
using Value = Google.Cloud.Datastore.V1.Value;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class EntityValueFactory
    {
        public static Value FromPropertyInfo(object poco, PropertyInfo prop, IEntityFactory entityFactory, IList<string> recursionPath)
        {
            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop), "Cannot build an entity Value without a property");
            }
            
            recursionPath.Add(prop.Name);
            
            var value = new Value
            {
                ExcludeFromIndexes = !prop.HasAttribute(typeof(IndexAttribute))
            };

            var propValue = prop.GetValue(poco);

            if (SetValue(prop.PropertyType, value, propValue, entityFactory, recursionPath)) return value;
                
            throw new NotSupportedException($"The type {prop.PropertyType.Name} is not supported");
        }

        private static bool SetValue(Type type, Value value, object propValue, IEntityFactory entityFactory, IList<string> recursionPath)
        {
            if (HandleNull(type, value, propValue, entityFactory)) return true;
            if (HandleEnum(type, value, propValue, entityFactory)) return true;
            if (HandleBinary(type, value, propValue, entityFactory)) return true;
            if (HandleString(type, value, propValue, entityFactory)) return true;
            if (HandleEnumerable(type, value, propValue, entityFactory, recursionPath)) return true;
            if (HandleBasicTypes(type, value, propValue, entityFactory)) return true;
            if (HandleComplexType(type, value, propValue, entityFactory, recursionPath)) return true;

            return false;
        }

        private static bool HandleComplexType(Type type, Value value, object propValue, IEntityFactory entityFactory, IList<string> recursionPath)
        {
            if (type.GetTypeInfo().IsClass)
            {
                value.EntityValue = entityFactory.EmbeddedEntityFromPoco(propValue, recursionPath);
                return true;
            }
            
            return false;
        }

        private static bool HandleString(Type type, Value value, object propValue, IEntityFactory entityFactory)
        {
            if (type == typeof(string))
            {
                value.StringValue = (string)propValue;
                return true;
            }

            return false;
        }


        private static bool HandleBasicTypes(Type type, Value value, object propValue, IEntityFactory entityFactory)
        {
            switch (type.Name)
            {
                case "Char":
                    value.StringValue = propValue.ToString();
                    break;
                
                case "Boolean":
                    value.BooleanValue = (bool)propValue;
                    break;
                
                case "Byte":
                    value.IntegerValue = (byte)propValue;
                    break;
                
                case "Int16":
                    value.IntegerValue = (short)propValue;
                    break;
                
                case "Int32":
                    value.IntegerValue = (int)propValue;
                    break;
                
                case "Int64":
                    value.IntegerValue = (long)propValue;
                    break;
                
                case "Single":
                    value.DoubleValue = double.Parse(propValue.ToString());
                    break;
                
                case "Double":
                    value.DoubleValue = (double)propValue;
                    break;
                
                case "Decimal":
                    value.DoubleValue = Convert.ToDouble(propValue);
                    break;
                
                case "DateTime":
                    value.TimestampValue = Timestamp.FromDateTime((DateTime)propValue);
                    break;
                
                case "DateTimeOffset":
                    value.TimestampValue = Timestamp.FromDateTimeOffset((DateTimeOffset)propValue);
                    break;
                
                case "TimeSpan":
                    value.IntegerValue = ((TimeSpan)propValue).Ticks;
                    break;
                
                default:
                    return false;
            }

            return true;
        }

        private static bool HandleEnumerable(Type type, Value value, object propValue, IEntityFactory entityFactory, IList<string> recursionPath)
        {
            if (type.ImplementsInterface(typeof(IEnumerable)))
            {
                value.ArrayValue = new ArrayValue();
                
                foreach (object item in (IEnumerable) propValue)
                {
                    var itemValue = new Value();
                    SetValue(item.GetType(), itemValue, item, entityFactory, recursionPath);
                    
                    value.ArrayValue.Values.Add(itemValue);
                }
                return true;
            }
            
            return false;
        }

        private static bool HandleBinary(Type type, Value value, object propValue, IEntityFactory entityFactory)
        {
            if (type.Name.Equals("Byte[]"))
            {
                var theBytes = (byte[]) propValue;
                value.BlobValue = ByteString.CopyFrom(theBytes, 0, theBytes.Length);
                return true;
            }

            return false;
        }

        private static bool HandleEnum(Type type, Value value, object propValue, IEntityFactory entityFactory)
        {
            if (type.GetTypeInfo().IsEnum)
            {
                value.StringValue = propValue.ToString();
                return true;
            }

            return false;
        }

        private static bool HandleNull(Type type, Value value, object propValue, IEntityFactory entityFactory)
        {
            if (propValue == null)
            {
                value.NullValue = NullValue.NullValue;
                return true;
            }

            return false;
        }
    }
}