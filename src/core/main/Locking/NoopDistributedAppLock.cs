using System;

namespace RapidCore.Locking
{
    /// <summary>
    /// <see cref="NoopDistributedAppLockProvider"/>
    /// </summary>
    [Obsolete("This should not be used in real systems", false)]
    public class NoopDistributedAppLock : IDistributedAppLock
    {
        public void Dispose()
        {
            IsActive = false;
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