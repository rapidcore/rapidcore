using System;

namespace RapidCore
{
    /// <summary>
    /// Just a generic "ignore" something attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class IgnoreAttribute : Attribute
    {
    }
}