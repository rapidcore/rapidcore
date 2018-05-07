using System;

namespace RapidCore.GoogleCloud.Datastore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class KindAttribute : Attribute
    {
        public virtual string Kind { get; }

        public KindAttribute(string kind)
        {
            Kind = kind;
        }
    }
}