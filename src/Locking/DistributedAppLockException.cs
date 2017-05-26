using System;

namespace RapidCore.Locking
{
    public class DistributedAppLockException : Exception
    {
        public DistributedAppLockException()
        {
        }

        public DistributedAppLockException(string message)
            : base(message)
        {
        }

        public DistributedAppLockException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}