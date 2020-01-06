namespace RapidCore.Reflection
{
    /// <summary>
    /// The result of calling either <see cref="IInstanceListener.OnField"/>
    /// or <see cref="IInstanceListener.OnProperty"/>
    /// </summary>
    public interface IInstanceListenerOnFieldOrPropResult
    {
        /// <summary>
        /// Whether the processing should continue
        /// recursing into this field or property
        /// </summary>
        bool DoContinueRecursion { get; }
    }
}