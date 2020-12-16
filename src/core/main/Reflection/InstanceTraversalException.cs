using System;

namespace RapidCore.Reflection
{
    [Serializable]
    public class InstanceTraversalException : Exception
    {
        public InstanceTraversalException(string message) : base(message)
        {
        }

        public InstanceTraversalException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}