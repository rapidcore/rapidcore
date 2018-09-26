using System.Reflection;
using RapidCore.Reflection;

namespace RapidCore.Diffing
{
    /// <summary>
    /// A change in value of a property between two object instances
    /// </summary>
    public class StateChange
    {
        /// <summary>
        /// The actual member. This is useful for doing
        /// custom filtering etc. of the changeset using
        /// custom attributes (e.g. in the context of audit logging)
        /// </summary>
        public virtual MemberInfo MemberInfo { get; set; }
        
        /// <summary>
        /// The name/path of the member
        /// </summary>
        public virtual string Breadcrumb { get; set; }

        private object oldValue = null;
        
        /// <summary>
        /// The old value
        ///
        /// If the value has not been set, then
        /// the default value of the type is
        /// returned - i.e. if MemberInfo points
        /// to an int field, 0 is returned
        /// </summary>
        public virtual object OldValue
        {
            get
            {
                if (oldValue == null)
                {
                    return GetDefaultValue();
                }

                return oldValue;
            }
            set
            {
                this.oldValue = value;
            }
        }
        
        private object newValue = null;
        
        /// <summary>
        /// The new value
        ///
        /// If the value has not been set, then
        /// the default value of the type is
        /// returned - i.e. if MemberInfo points
        /// to an int field, 0 is returned
        /// </summary>
        public virtual object NewValue
        {
            get
            {
                if (newValue == null)
                {
                    return GetDefaultValue();
                }

                return newValue;
            }
            set
            {
                this.newValue = value;
            }
        }

        protected virtual object GetDefaultValue()
        {
            return MemberInfo.GetTypeOfValue().GetDefaultValue();
        }
    }
}