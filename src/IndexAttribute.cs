using System;

namespace RapidCore.Mongo
{
    /// <summary>
    /// Declare property as being part of an Index
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public IndexAttribute()
        {
        }

        public IndexAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the index. Should only be used
        /// for compound indexes
        /// </summary>
        public string Name { get; set; }
        public int Order { get; set; } = 0;
        public bool Sparse { get; set; } = false;
        
        /// <summary>
        /// Determines whether the index is a unique index constraint
        /// </summary>
        public bool Unique { get; set; } = false;
    }
}