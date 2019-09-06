using System;

namespace RapidCore.Locking
{
    /// <summary>
    /// <see cref="NoopDistributedAppLockProvider"/>
    /// </summary>
    [Obsolete("This should not be used in real systems", false)]
    public class NoopDistributedAppLock : IDistributedAppLock
    {
        private bool disposed = false;
        
        protected virtual void Dispose(bool disposing)
        {
            // Do not dispose this object multiple times
            if (this.disposed)
            {
                return;
            }
            
            // IsActive does not reference other objects and
            // can therefore be altered even when disposing = false
            IsActive = false;
                
            // Mark this object as disposed (so it does not happen twice)
            this.disposed = true;
        }
    
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Determines whether the current lock instance is <see cref="IsActive"/> and has a name that matches the given
        /// parameter
        /// </summary>
        /// <param name="name">The name of the lock to asssert that is currently taken</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void ThrowIfNotActiveWithGivenName(string name)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=false with name '{this.Name}'");
            }

            if (!Name.Equals(name))
            {
                throw new InvalidOperationException(
                    $"Lock precondition mismatch, required IsActive=true with name '{name}' but IsActive=true with name '{this.Name}'");
            }
        }
    }
}