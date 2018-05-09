using System;

namespace RapidCore.GoogleCloud.Datastore
{
    /// <summary>
    /// This property should be indexed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
    }
}