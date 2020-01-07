using System;

namespace RapidCore
{
    /// <summary>
    /// Use when code failed to resolve something from
    /// a DI container or whatever else makes semantic sense.
    /// </summary>
    [Serializable]
    public class FailureToResolveException : Exception
    {
        public FailureToResolveException(string message) : base(message)
        {
        }

        public FailureToResolveException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}