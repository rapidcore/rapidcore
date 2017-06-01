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

        public DistributedAppLockException(string message, DistributedAppLockExceptionReason reason)
            : base(message)
        {
            Reason = reason;
        }

        public DistributedAppLockException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DistributedAppLockException(string message, Exception inner, DistributedAppLockExceptionReason reason)
            : base(message, inner)
        {
            Reason = reason;
        }

        public DistributedAppLockExceptionReason Reason { get; set; }
    }
}