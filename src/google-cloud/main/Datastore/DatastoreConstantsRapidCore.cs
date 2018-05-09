namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// Constants for the Datastore stack
    /// </summary>
    public static class DatastoreConstantsRapidCore
    {
        /// <summary>
        /// Recursion level, at which the ORM bails out
        /// in order to prevent StackOverflowException
        /// </summary>
        public static readonly int MaxRecursionDepth = 20;
    }
}