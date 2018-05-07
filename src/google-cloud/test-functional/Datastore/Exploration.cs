using System;
using System.Collections.Generic;
using Google.Cloud.Datastore.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RapidCore.GoogleCloud.Datastore;
using Xunit;
using Value = Google.Cloud.Datastore.V1.Value;

namespace functionaltests.csproj.Datastore
{
    public class Exploration
    {
        private readonly DatastoreConnection dsConnection;

        public Exploration()
        {
            var client = DatastoreClient.Create(
                new Channel("localhost", 8081, ChannelCredentials.Insecure),
                DatastoreSettings.GetDefault()
            );
            
            var dsDb = DatastoreDb.Create("project-id-12345", "test-space", client);
            dsConnection = new DatastoreConnection(dsDb);
        }

        [Fact]
        public async void Muhahahaha()
        {
            /**
             * boolTrue
             * boolFalse
             * char
             * string
             * byte
             * sbyte
             * short
             * ushort
             * int
             * uint
             * long
             * ulong
             * float
             * double
             * decimal
             * enum
             * datetime
             * timestamp
             * list
             * array
             * byte[]
             * complex
             * null
             */
            var myDecimal = 1.123456789012345m;

            var myBytes = new byte[] {1, 2, 3, 4, 5};
            
            var entity = new Entity
            {
                Key = dsConnection.DatastoreDb.CreateKeyFactory("cokeysmurf").CreateKey(Guid.NewGuid().ToString()),
                ["myBoolTrue"] = new Value
                {
                    BooleanValue = true
                },
                ["myBoolFalse"] = new Value
                {
                    BooleanValue = false
                },
                ["myChar"] = new Value
                {
                    StringValue = 'c'.ToString()
                },
                ["myString"] = new Value
                {
                    StringValue = "slim shady"
                },
                ["myByte"] = new Value
                {
                    IntegerValue = byte.Parse("14")
                },
                ["mySbyte"] = new Value
                {
                    IntegerValue = sbyte.Parse("-14")
                },
                ["myShort"] = new Value
                {
                    IntegerValue = short.Parse("-5")
                },
                ["myUshort"] = new Value
                {
                    IntegerValue = ushort.Parse("5")
                },
                ["myInt"] = new Value
                {
                    IntegerValue = -666
                },
                ["myUint"] = new Value
                {
                    IntegerValue = 666
                },
                ["myLong"] = new Value
                {
                    IntegerValue = -666
                },
                ["myUlong"] = new Value
                {
                    IntegerValue = 666
                },
                ["myFloat"] = new Value
                {
                    DoubleValue = 6.66F
                },
                ["myDouble"] = new Value
                {
                    DoubleValue = 6.667788
                },
                ["myDecimal"] = new Value
                {
                    DoubleValue = Convert.ToDouble(myDecimal)
                },
                ["myEnum"] = new Value
                {
                    StringValue = KewlEnum.One.ToString()
                },
                ["myDatetime"] = new Value
                {
                    TimestampValue = Timestamp.FromDateTime(DateTime.UtcNow) 
                },
                ["myTimestamp"] = new Value
                {
                    IntegerValue = new TimeSpan(1, 13, 25, 46, 789).Ticks
                },
                ["myList"] = new Value
                {
                    ArrayValue = new ArrayValue(new List<string> {"one", "two", "three"}.ToArray())
                },
                ["myArray"] = new Value
                {
                    ArrayValue = new ArrayValue(new long[] { 1, 2, 3 })
                },
                /*
                 * Datastore does not have integers (they are longs).
                 * For now we consider this case out of scope!
                ["myIntArray"] = new Value
                {
                    ArrayValue = new ArrayValue(new int[] { 1, 2, 3 })
                },*/
                ["myByteArrayAkaBinary"] = new Value
                {
                    BlobValue = ByteString.CopyFrom(myBytes, 0, myBytes.Length)
                },
                ["myComplex"] = new Value
                {
                    EntityValue = new Entity
                    {
                        ["prop1"] = new Value
                        {
                            StringValue = "prop 1 value"
                        },
                        ["prop2"] = new Value
                        {
                            StringValue = "prop 2 value"
                        }
                    }
                },
                ["myNull"] = new Value
                {
                    NullValue = NullValue.NullValue 
                }
            };
            
            
            await dsConnection.DatastoreDb.InsertAsync(entity);
        }
    }
}