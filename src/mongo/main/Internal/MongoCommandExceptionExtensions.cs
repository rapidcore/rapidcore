using System;
using MongoDB.Driver;

namespace RapidCore.Mongo.Internal
{
    public static class MongoCommandExceptionExtensions
    {
        public static bool EnsureIndexesShouldDropAndRetry(this MongoCommandException ex, IndexDefinition index)
        {
            //
            // index already exists, but with different options
            // Check using the error code - all error codes are defined here and have been stable over time, with only new codes added:
            // https://github.com/mongodb/mongo/blob/v3.4/src/mongo/base/error_codes.err
            //
            if (ex.Code == 86)
            {
                return true;
            }
            
            //
            // index already exists
            //
            else if (ex.Message.StartsWith("Command createIndexes failed: Index must have unique name.The existing index:"))
            {
                // check if the existing and the requested index are different

                var indexes = ex.Message
                    .Remove(0, "Command createIndexes failed: Index must have unique name.The existing index: ".Length)
                    .Split(new string[] { " has the same name as the requested index: " }, StringSplitOptions.None);

                return !indexes[0].Trim().Equals(indexes[1].Trim());
            }

            return false;
        }
    }
}