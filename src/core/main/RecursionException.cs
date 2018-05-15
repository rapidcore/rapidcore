using System;

namespace RapidCore
{
    public class RecursionException : Exception
    {
        public RecursionException(string message) : base(message)
        {
        }

        public RecursionException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}