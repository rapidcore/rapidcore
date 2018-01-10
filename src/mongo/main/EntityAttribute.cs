using System;

namespace RapidCore.Mongo
{
    /// <summary>
    /// Flags a class as being an entity
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class EntityAttribute : Attribute
    {
    }
}