using System;

namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// Marks this property as the ID or primary key
    /// of the entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
    }
}