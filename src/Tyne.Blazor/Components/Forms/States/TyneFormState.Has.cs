namespace Tyne.Blazor;

public static partial class TyneFormState
{
    /// <summary>
    ///     Behaviours which describe what a state may have.
    /// </summary>
    public static class Has
    {
        /// <summary>
        ///     Form has been initialised.
        /// </summary>
        public interface Initialised<out TParams>
        {
            public TParams Params { get; }
        }

        /// <summary>
        ///     Form has a blocking error, ie it cannot continue.
        /// </summary>
        /// <remarks>
        ///     This differs from <see cref="EphemeralError{TError}"/>, which has an error which can be resolved.
        /// </remarks>
        public interface BlockingError<out TError>
        {
            public TError Error { get; }
        }

        /// <summary>
        ///     Form has an ephemeral error which may be resolved.
        /// </summary>
        /// <remarks>
        ///     The error may be null, and may be updated.
        /// </remarks>
        public interface EphemeralError<TError>
        {
            public TError? Error { get; }

            public Task UpdateErrorAsync(TError? error);
        }

        /// <summary>
        ///     Form has an active model.
        /// </summary>
        public interface Model<out TModel>
        {
            public TModel Model { get; }
        }
    }
}
