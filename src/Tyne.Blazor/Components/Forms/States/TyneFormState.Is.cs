namespace Tyne.Blazor;

public static partial class TyneFormState
{
    /// <summary>
    ///     Behaviours which describe what a state is.
    /// </summary>
    public static class Is
    {
        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Initialising"/></item>
        ///     </list>
        /// </remarks>
        public interface Uninitialised :
            ITyneFormState;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Initialising"/></item>
        ///     </list>
        /// </remarks>
        public interface Uninitialised<TParams, TError> :
            Uninitialised,
            Supports.Initialising<TParams, TError>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="InitialisationError"/> if errored</item>
        ///         <item><see cref="Inactive"/> if ok</item>
        ///     </list>
        /// </remarks>
        public interface Initialising :
            ITyneFormState;

        /// <remarks>
        ///     Cannot transition away from this state.
        /// </remarks>
        public interface InitialisationError :
            ITyneFormState;

        /// <remarks>
        ///     Cannot transition away from this state.
        /// </remarks>
        public interface InitialisationError<out TError> :
            InitialisationError,
            Has.BlockingError<TError>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Loading"/></item>
        ///     </list>
        /// </remarks>
        public interface Inactive :
            ITyneFormState;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Loading"/></item>
        ///     </list>
        /// </remarks>
        public interface Inactive<out TParams, TModel, TError> :
            Inactive,
            Has.Initialised<TParams>,
            Supports.MakingActive<TModel, TError>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Active"/> if ok</item>
        ///         <item><see cref="LoadingError"/> if errored</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Loading :
            ITyneFormState,
            Supports.MakingInactive;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Active"/> if ok</item>
        ///         <item><see cref="LoadingError"/> if errored</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Loading<out TParams> :
            Loading,
            Has.Initialised<TParams>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Inactive"/></item>
        ///     </list>
        /// </remarks>
        public interface LoadingError :
            ITyneFormState,
            Supports.MakingInactive;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Inactive"/></item>
        ///     </list>
        /// </remarks>
        public interface LoadingError<out TParams, out TError> :
            LoadingError,
            Has.Initialised<TParams>,
            Has.BlockingError<TError>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Saving"/> if saved</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Active :
            ITyneFormState,
            Supports.Saving,
            Supports.MakingInactive;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Saving"/> if saved</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Active<out TParams, out TModel, TError> :
            Active,
            Has.Initialised<TParams>,
            Has.Model<TModel>,
            Has.EphemeralError<TError>;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Active"/> if ok or error (see <see cref="Has.EphemeralError{TError}"/>)</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Saving :
            ITyneFormState;

        /// <remarks>
        ///     Can transition to:
        ///     <list type="bullet">
        ///         <item><see cref="Active"/> if ok or error (see <see cref="Has.EphemeralError{TError}"/>)</item>
        ///         <item><see cref="Inactive"/> if cancelled</item>
        ///     </list>
        /// </remarks>
        public interface Saving<out TParams, out TModel> :
            Saving,
            Has.Initialised<TParams>,
            Has.Model<TModel>;

        /// <summary>
        ///     A partial union between <see cref="Active"/> and <see cref="Saving"/>.
        /// </summary>
        /// <remarks>
        ///     This only contains behaviours which are common between active and saving.
        /// </remarks>
        public interface Ready :
            ITyneFormState;

        /// <summary>
        ///     A partial union between <see cref="Active{TParams, TModel, TError}"/> and <see cref="Saving{TParams,TModel}"/>.
        /// </summary>
        /// <remarks>
        ///     This only contains behaviours which are common between active and saving.
        /// </remarks>
        public interface Ready<out TParams, out TModel, TError> :
            Ready,
            Has.Initialised<TParams>,
            Has.Model<TModel>,
            Has.EphemeralError<TError>;
    }
}
