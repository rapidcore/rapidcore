using System;

namespace RapidCore.Mongo
{
    /// <summary>
    /// Declare property as being part of an Index
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        public IndexAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public int Order { get; set; } = 0;
        public bool Sparse { get; set; } = false;
    }
}