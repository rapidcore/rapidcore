using System;

namespace RapidCore.GoogleCloud.Datastore
{
    public class PrimaryKeyException : Exception
    {
        public PrimaryKeyException(string message) : base(message)
        {
        }
        
        public PrimaryKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}