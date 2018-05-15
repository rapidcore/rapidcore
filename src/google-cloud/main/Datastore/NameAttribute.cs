using System;

namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// Set the name to use for this property in
    /// datastore entities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}