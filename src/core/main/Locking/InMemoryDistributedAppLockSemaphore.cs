using System.Threading;

namespace RapidCore.Locking
{
    public class InMemoryDistributedAppLockSemaphore : SemaphoreSlim
    {
        public int ReferenceCount { get; private set; }

        public InMemoryDistributedAppLockSemaphore() : base(1, 1)
        {
            ReferenceCount = 0;
        }

        public void IncrementReferenceCount()
        {
            lock (this)
            {
                ++ReferenceCount;
            }
        }

        public void DecrementReferenceCount()
        {
            lock (this)
            {
                --ReferenceCount;
            }
        }
    }
}