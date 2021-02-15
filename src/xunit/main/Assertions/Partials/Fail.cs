namespace RapidCore.Xunit.Assertions
{
    public static partial class RapidCoreAssert
    {
        /// <summary>
        /// Make a test fail.
        ///
        /// This is useful when a line in a test should
        /// never be reached - e.g. when testing exception
        /// handling with try..catch
        /// </summary>
        /// <param name="reason">The reason why this should fail</param>
        public static void Fail(string reason)
        {
            throw new FailXunitException(reason);
        }
    }
}