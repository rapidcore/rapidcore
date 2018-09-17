namespace RapidCore.Audit
{
    /// <summary>
    /// Mask a given value in the context of <see cref="AuditDiffer"/>
    /// </summary>
    public interface IAuditValueMasker
    {
        string MaskValue(object value);
    }
}