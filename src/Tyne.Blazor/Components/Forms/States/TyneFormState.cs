using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor;

/// <summary>
///     Describes <see cref="ITyneFormState"/>s.
/// </summary>
// Analysers HATE this pattern, but organising the state interfaces under one class
// (and omitting the I... prefix) makes discovering/using them much easier/cleaner for consumers.
[SuppressMessage("Design", "CA1034: Nested types should not be visible.")]
[SuppressMessage("Design", "CA1040: Avoid empty interfaces.")]
[SuppressMessage("Naming", "CA1715: Identifiers should have correct prefix.")]
[SuppressMessage("Minor Code Smell", "S101: Types should be named in PascalCase.")]
[SuppressMessage("Critical Code Smell", "S3218: Inner class members should not shadow outer class \"static\" or type members.")]
public static partial class TyneFormState
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
        Is.Ready,
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
        Is.Ready<TParams, TModel, TError>;

    /// <remarks>
    ///     Can transition to:
    ///     <list type="bullet">
    ///         <item><see cref="Active"/> if ok or error (see <see cref="Has.EphemeralError{TError}"/>)</item>
    ///         <item><see cref="Inactive"/> if cancelled</item>
    ///     </list>
    /// </remarks>
    public interface Saving :
        Is.Ready,
        Is.Saving;

    /// <remarks>
    ///     Can transition to:
    ///     <list type="bullet">
    ///         <item><see cref="Active"/> if ok or error (see <see cref="Has.EphemeralError{TError}"/>)</item>
    ///         <item><see cref="Inactive"/> if cancelled</item>
    ///     </list>
    /// </remarks>
    public interface Saving<out TParams, out TModel, TError> :
        Saving,
        Is.Ready<TParams, TModel, TError>,
        Is.Saving<TParams, TModel>;
}
