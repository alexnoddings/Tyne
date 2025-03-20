namespace Tyne.Blazor;

public static partial class TyneFormState
{
    /// <summary>
    ///     Behaviours which describe what a state may support.
    /// </summary>
    public static class Supports
    {
        /// <summary>
        ///     Form supports being initialised.
        /// </summary>
        public interface Initialising<TParams, TError>
        {
            public Task InitialiseAsync(InitialiseForm<TParams, TError> initialiseForm);
        }

        /// <summary>
        ///     Form supports being saved.
        /// </summary>
        public interface Saving
        {
            public Task SaveAsync();
        }

        /// <summary>
        ///     Form supports being made inactive.
        /// </summary>
        public interface MakingInactive
        {
            public Task MakeInactiveAsync();
        }

        /// <summary>
        ///     Form supports being made active.
        /// </summary>
        public interface MakingActive<TModel, TError>
        {
            public Task MakeActiveAsync(MakeFormActive<TModel, TError> makeFormActive);
        }
    }
}
