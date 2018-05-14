using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Google.Cloud.Datastore.V1;
using RapidCore.Reflection;

namespace RapidCore.GoogleCloud.Datastore.Internal
{
    public class PocoValueFactory
    {
        public static object FromEntityValue(PropertyInfo prop, Value value, IPocoFactory pocoFactory)
        {
            if (prop == null)
            {
                throw new ArgumentNullException(nameof(prop), "Cannot extract the proper data from the Value without having the property");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), "Cannot extract data from a null Value");
            }

            return GetValue(prop.PropertyType.GetTypeOrUnderlyingNullableType(), value, pocoFactory);
        }

        private static object GetValue(Type type, Value value, IPocoFactory pocoFactory)
        {
            Tuple<bool, object> x;

            x = HandleNull(type, value); if (x.Item1) return x.Item2;
            x = HandleEnum(type, value); if (x.Item1) return x.Item2;
            x = HandleBinary(type, value); if (x.Item1) return x.Item2;
            x = HandleString(type, value); if (x.Item1) return x.Item2;
            x = HandleEnumerable(type, value, pocoFactory); if (x.Item1) return x.Item2;
            x = HandleBasicTypes(type, value); if (x.Item1) return x.Item2;
            x = HandleComplexType(type, value, pocoFactory); if (x.Item1) return x.Item2;
            
            throw new NotSupportedException($"The type {type.Name} is not supported");
        }

        private static Tuple<bool, object> HandleBasicTypes(Type type, Value value)
        {
            switch (type.Name)
            {
                case "Char":
                    return new Tuple<bool, object>(true, value.StringValue[0]);
                
                case "Boolean":
                    return new Tuple<bool, object>(true, value.BooleanValue);
                
                case "Byte":
                    return new Tuple<bool, object>(true, Convert.ToByte(value.IntegerValue));
                
                case "Int16":
                    return new Tuple<bool, object>(true, Convert.ToInt16(value.IntegerValue));
                
                case "Int32":
                    return new Tuple<bool, object>(true, Convert.ToInt32(value.IntegerValue));
                
                case "Int64":
                    return new Tuple<bool, object>(true, value.IntegerValue);
                
                case "Single":
                    return new Tuple<bool, object>(true, Convert.ToSingle(value.DoubleValue));
                
                case "Double":
                    return new Tuple<bool, object>(true, value.DoubleValue);
                
                case "Decimal":
                    var dec = Convert.ToDecimal(value.EntityValue["integral"].IntegerValue);
                    dec = dec + Convert.ToDecimal(value.EntityValue["fraction"].DoubleValue);
                    return new Tuple<bool, object>(true, dec);
                
                case "DateTime":
                    return new Tuple<bool, object>(true, value.TimestampValue.ToDateTime());
                
                case "DateTimeOffset":
                    return new Tuple<bool, object>(true, value.TimestampValue.ToDateTimeOffset());
                
                case "TimeSpan":
                    return new Tuple<bool, object>(true, TimeSpan.FromTicks(value.IntegerValue));
                
                default:
                    return new Tuple<bool, object>(false, null);
            }
        }

        private static Tuple<bool, object> HandleEnumerable(Type type, Value value, IPocoFactory pocoFactory)
        {
            if (type.ImplementsInterface(typeof(IEnumerable)))
            {
                var values = value.ArrayValue.Values;
                var elmType = type.GetElementType();
                
                if (type.IsArray)
                {
                    var output = Array.CreateInstance(elmType, values.Count);

                    for (var i = 0; i < output.Length; i++)
                    {
                        output.SetValue(GetValue(elmType, values[i], pocoFactory), i);
                    }
                    
                    return new Tuple<bool, object>(true, output);
                }


                elmType = type.GenericTypeArguments.FirstOrDefault();
                if (elmType == null)
                {
                    throw new NotSupportedException($"I am unable to recreate a ${type.Name}");
                }
                
                var data = (IList)Activator.CreateInstance(type);
                
                foreach (var item in values)
                {
                    data.Add(GetValue(elmType, item, pocoFactory));
                }
                
                return new Tuple<bool, object>(true, data);
            }
            
            return new Tuple<bool, object>(false, null);
        }

        private static Tuple<bool, object> HandleBinary(Type type, Value value)
        {
            if (type.Name.Equals("Byte[]"))
            {
                return new Tuple<bool, object>(true, value.BlobValue.ToByteArray());
            }

            return new Tuple<bool, object>(false, null);
        }

        private static Tuple<bool, object> HandleComplexType(Type type, Value value, IPocoFactory pocoFactory)
        {
            if (type.GetTypeInfo().IsClass)
            {
                var obj = pocoFactory.FromEmbeddedEntity(type, value.EntityValue);
                return new Tuple<bool, object>(true, obj);
            }
            
            return new Tuple<bool, object>(false, null);
        }

        private static Tuple<bool, object> HandleString(Type type, Value value)
        {
            if (type == typeof(string))
            {
                return new Tuple<bool, object>(true, value.StringValue);
            }

            return new Tuple<bool, object>(false, null);
        }

        private static Tuple<bool, object> HandleEnum(Type type, Value value)
        {
            if (type.GetTypeInfo().IsEnum)
            {
                var x = Enum.Parse(type, value.StringValue, true);
                
                return new Tuple<bool, object>(true, x);
            }

            return new Tuple<bool, object>(false, null);
        }

        private static Tuple<bool, object> HandleNull(Type type, Value value)
        {
            return new Tuple<bool, object>(value.IsNull, null);
        }
    }
}