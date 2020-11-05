using Xunit.Sdk;

namespace RapidCore.Xunit.Assertions
{
    /// <summary>
    /// The exception to throw from <see cref="RapidCoreAssert.Fail"/>
    /// </summary>
    public class FailXunitException : XunitException
    {
        public FailXunitException(string reason) : base($"Forced failure: {reason}")
        {
        }
    }
}