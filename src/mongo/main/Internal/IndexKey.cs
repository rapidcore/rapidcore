namespace RapidCore.Mongo.Internal
{
    /// <summary>
    /// Represents a "key" in Mongo DB terms
    /// </summary>
    public class IndexKey
    {
        public virtual string Name { get; set; }
        public virtual int Order { get; set; }
    }
}