namespace RapidCore.Audit
{
    /// <summary>
    /// Always outputs the same series of stars, so
    /// it does not reveal anything about the value itself -
    /// not even the length.
    /// </summary>
    public class PasswordAuditValueMasker : IAuditValueMasker
    {
        public string MaskValue(object value)
        {
            return "******";
        }
    }
}