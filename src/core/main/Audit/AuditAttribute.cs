using System;

namespace RapidCore.Audit
{
    /// <summary>
    /// Control how this class, field, property or struct
    /// is handled by the <see cref="AuditDiffer"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AuditAttribute : Attribute
    {
        /// <summary>
        /// Whether or not to include this thing in
        /// the audit diff.
        ///
        /// Default value is <c>true</c>.
        /// </summary>
        public virtual bool Include { get; set; } = true;
        
        /// <summary>
        /// A type that implements <see cref="IAuditValueMasker"/>, which
        /// will be used to modify the value of both old and new state
        /// for the log
        /// </summary>
        public virtual Type ValueMasker { get; set; }

        /// <summary>
        /// Convenience method to see if a masker
        /// has been supplied
        /// </summary>
        public virtual bool DoMaskValue => ValueMasker != null;
    }
}