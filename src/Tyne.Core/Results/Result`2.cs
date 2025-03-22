using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Tyne.Utilities;

namespace Tyne;

/// <summary>
///     A result encapsulates either <c>Ok(<typeparamref name="T"/>)</c> or <c>Error(<typeparamref name="TE"/>)</c>.
/// </summary>
/// <typeparam name="T">The type of value this result encapsulates.</typeparam>
/// <typeparam name="TE">The type of error this result encapsulates.</typeparam>
/// <seealso cref="Result"/>
/// <seealso cref="ResultExtensions"/>
/// <seealso cref="ResultJsonConverterFactory"/>
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(Result<,>.DebuggerTypeProxy))]
[JsonConverter(typeof(ResultJsonConverterFactory))]
public sealed partial class Result<T, TE> : IEquatable<Result<T, TE>>
{
    private readonly bool _isOk;
    private readonly T? _value;
    private readonly TE? _error;

    internal Result([DisallowNull] T value)
    {
        _isOk = true;
        _value = value;
    }

    internal Result([DisallowNull] TE error)
    {
        _isOk = false;
        _error = error;
    }

    /// <summary>
    ///     Tries to unwrap this result.
    /// </summary>
    /// <param name="ok">
    ///     This result's Ok value if it is Ok(<typeparamref name="T"/>); otherwise, <see langword="default"/> when it is Error(<typeparamref name="TE"/>).
    /// </param>
    /// <param name="error">
    ///     This result's Error value if it is Error(<typeparamref name="TE"/>); otherwise, <see langword="default"/> when it is Ok(<typeparamref name="T"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if this is Ok(<typeparamref name="T"/>);
    ///     otherwise, <see langword="false"/> when it is Error(<typeparamref name="TE"/>).
    /// </returns>
    /// <remarks>
    ///     If this is Ok(<typeparamref name="T"/>), then <paramref name="ok"/> is set and this returns <see langword="true"/>.
    ///     Otherwise, when this is Error(<typeparamref name="TE"/>), then <paramref name="error"/> will be set and this returns <see langword="false"/>.
    /// </remarks>
    [Pure]
    public bool TryUnwrap(
        [NotNullWhen(true)] out T? ok,
        [NotNullWhen(false)] out TE? error
    )
    {
        if (_isOk)
        {
            ok = _value!;
            error = default;
            return true;
        }

        ok = default;
        error = _error!;
        return false;
    }

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///		    A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    ///		</para>
    ///     <para>
    ///         If this is <c>Ok(<typeparamref name="T"/>)</c>, then this returns the <typeparamref name="T"/> value's hash code.
    ///         Otherwise, if it is <c>Error(<typeparamref name="TE"/>)</c>, it returns the <typeparamref name="TE"/> error's hash code.
    ///     </para>
    /// </returns>
    [Pure]
    public override int GetHashCode() =>
        _isOk
        ? _value!.GetHashCode()
        : _error!.GetHashCode();

    /// <summary>
    ///		Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <remarks>
    ///     If this is <c>Ok(<typeparamref name="T"/>)</c>, this returns <c>Ok({value})</c>.
    ///     Otherwise, it returns <c>Error({error})</c>.
    /// </remarks>
    [Pure]
    public override string ToString() =>
        _isOk
            ? TyneToStringHelpers.CreateStringFor("Ok", _value!)
            : TyneToStringHelpers.CreateStringFor("Error", _error!);

    /// <summary>
    ///     Implicitly converts <paramref name="result"/> into a <see cref="Result{T, TE}"/> of type <see cref="Unit"/>.
    /// </summary>
    /// <param name="result">The <see cref="Result{T, TE}"/> to convert.</param>
    /// <remarks>
    ///     This is useful to discard the generic value from a result,
    ///     such as when passing it into a method that only cares about success/failure.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<Unit, TE>(in Result<T, TE> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (result._isOk)
            return Result.Cache<TE>.OkUnit;

        return new Result<Unit, TE>(result._error!);
    }

    /// <summary>
    ///     Implicitly converts <paramref name="value"/> into an Ok(<typeparamref name="T"/>) <see cref="Result{T, TE}"/>.
    /// </summary>
    /// <param name="value">
    ///     The Ok value to encapsulate.
    /// </param>
    /// <returns>
    ///     An Ok(<typeparamref name="T"/>) result which encapsulates <paramref name="value"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T, TE>([DisallowNull] in T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return new Result<T, TE>(value);
    }

    /// <summary>
    ///     Implicitly converts <paramref name="error"/> into an Error(<typeparamref name="TE"/>) <see cref="Result{T, TE}"/>.
    /// </summary>
    /// <param name="error">
    ///     The Error value to encapsulate.
    /// </param>
    /// <returns>
    ///     An Error(<typeparamref name="TE"/>) result which encapsulates <paramref name="error"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<T, TE>([DisallowNull] in TE error)
    {
        ArgumentNullException.ThrowIfNull(error);

        return new Result<T, TE>(error);
    }

    /// <summary>
    ///     Converts <paramref name="result"/> into an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="result">The <see cref="Result{T, TE}"/> to convert.</param>
    /// <remarks>
    ///     This is useful to discard the error value from a result.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<T>(in Result<T, TE>? result)
    {
        if (result?._isOk == true)
            return Option.Some(result._value!);

        return Option.None<T>();
    }

    // Debugger proxy exposes _value/_error more cleanly.
    [ExcludeFromCodeCoverage]
    [SuppressMessage(
        "Major Code Smell",
        "S1144: Unused private types or members should be removed",
        Justification = "These members are used by the debugger."
    )]
    private sealed class DebuggerTypeProxy
    {
        public bool IsOk { get; }

        public T? Value { get; }
        public TE? Error { get; }

        public DebuggerTypeProxy(Result<T, TE> result)
        {
            IsOk = result._isOk;
            Value = result._value;
            Error = result._error;
        }
    }
}
