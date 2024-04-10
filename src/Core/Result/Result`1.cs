using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     A result encapsulates either <c>Ok(<typeparamref name="T"/>)</c> or <c>Error</c>.
///     <list type="bullet">
///         <item>
///             In the <c>Ok(<typeparamref name="T"/>)</c> case, a result holds some <see cref="Value"/>,
///             and <see cref="IsOk"/> is <see langword="true"/>.
///             Accessing <see cref="Error"/> will throw a <see cref="BadResultException"/>.
///         </item>
///         <item>
///             In the <c>Error</c> case, a result holds some <see cref="Error"/>,
///             and <see cref="IsOk"/> is <see langword="false"/>.
///             Accessing <see cref="Value"/> will throw a <see cref="BadResultException"/>.
///         </item>
///     </list>
/// </summary>
/// <remarks>
///     <para>
///         The purpose of <see cref="Error"/> is to provide a strong construct for handling errors without <see cref="Exception"/>s.
///         This encourages consumers to consider how to handle a bad result, rather than always assuming the happy path.
///     </para>
///     <para>
///         See <see cref="Result"/> for how to create <see cref="Result{T}"/>s.
///     </para>
/// </remarks>
/// <typeparam name="T">The type of value this result encapsulates.</typeparam>
/// <seealso cref="Result"/>
/// <seealso cref="ResultExtensions"/>
/// <seealso cref="ResultJsonConverterFactory"/>
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(Result<>.DebuggerTypeProxy))]
[JsonConverter(typeof(ResultJsonConverterFactory))]
public class Result<T> : IEquatable<Result<T>>
{
    private readonly bool _isOk;
    private readonly T? _value;
    private readonly Error? _error;

    /// <summary>
    ///     Indicates whether this result is <c>Ok(<typeparamref name="T"/>)</c> (i.e. has a value).
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if this result is <c>Ok(<typeparamref name="T"/>)</c>;
    ///     otherwise, <see langword="false"/> if it is <c>Error</c>.
    /// </returns>
    [Pure]
    public bool IsOk => _isOk;

    /// <summary>
    ///     The unwrapped <typeparamref name="T"/> which this result encapsulates, if it is <c>Ok(<typeparamref name="T"/>)</c>.
    /// </summary>
    /// <returns>
    ///     The <typeparamref name="T"/> which this result encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If this result is <c>Error</c>, unwrapping this will instead throw a <see cref="BadResultException"/>.
    ///         You should ensure <see cref="IsOk"/> is <see langword="true"/> before accessing this property.
    ///     </para>
    ///     <para>
    ///         Alternatively, consider using one of <see cref="ResultExtensions"/> to access this value,
    ///         such as <see cref="ResultExtensions.Or{T}(Result{T}, T)"/> or <see cref="ResultExtensions.Unwrap{T}(Result{T})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="BadResultException">When this result is <c>Error</c>.</exception>
    [Pure]
    public T Value
    {
        get
        {
            if (!_isOk)
                throw new BadResultException(ExceptionMessages.Result_ErrorHasNoValue);

            return _value!;
        }
    }

    /// <summary>
    ///     The <see cref="Tyne.Error"/> which this result encapsulates, if it is <c>Error</c>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If this result is <c>Ok(<typeparamref name="T"/>)</c>, accessing this will instead throw a <see cref="BadResultException"/>.
    ///         You should ensure <see cref="IsOk"/> is <see langword="false"/> before accessing this property.
    ///     </para>
    ///     <para>
    ///         Alternatively, consider using one of <see cref="ResultExtensions"/> to access this value.
    ///     </para>
    /// </remarks>
    /// <exception cref="BadResultException">When this result is <c>Ok(<typeparamref name="T"/>)</c>.</exception>
    [Pure]
    public Error Error
    {
        get
        {
            if (_isOk)
                throw new BadResultException(ExceptionMessages.Result_OkHasNoError);

            return _error!;
        }
    }

    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The <see cref="Result{T}.Value"/>. Cannot be <see langword="null"/>.</param>
    /// <exception cref="BadResultException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <remarks>
    ///     This constructor is only available to inheritors.
    ///     External callers must use <see cref="Result.Ok{T}(in T)"/> (or similar for a derived result).
    /// </remarks>
    protected internal Result(T value)
    {
        if (value is null)
            throw new BadResultException(ExceptionMessages.Result_OkMustHaveValue);

        _isOk = true;
        _value = value;
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error">The <see cref="Result{T}.Error"/>. Cannot be <see langword="null"/>.</param>
    /// <exception cref="BadResultException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    /// <remarks>
    ///     This constructor is only available to inheritors.
    ///     External callers must use <see cref="Result.Error{T}(in Error)"/> (or similar for a derived result).
    /// </remarks>
    protected internal Result(Error error)
    {
        if (error is null)
            throw new BadResultException(ExceptionMessages.Result_ErrorMustHaveError);

        _isOk = false;
        _error = error;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is a <see cref="Result{T}"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="Result{T}"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Result<T> other)
            return Equals(other);

        return false;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="Result{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Two <see cref="Result{T}"/>s are equal if
    ///         both are <c>Ok(<typeparamref name="T"/>)</c> and their <see cref="Value"/>s are equal,
    ///         or if both are <c>Error</c> and their <see cref="Result{T}"/>s are equal.
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined by calling <see cref="object.Equals(object?)"/>.
    ///         If <typeparamref name="T"/> does not implement this, it may not behave as expected.
    ///     </para>
    /// </remarks>
    [Pure]
    public virtual bool Equals([NotNullWhen(true)] Result<T>? other)
    {
        if (other is null)
            return false;

        // Both are Ok(T), compare T values
        if (_isOk && other._isOk)
            return _value!.Equals(other._value);

        // Only one is Ok(T), then they cannot be equal
        if (_isOk || other._isOk)
            return false;

        return _error!.Equals(other._error);
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Result{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Result{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Result{T})"/> for how <see cref="Result{T}"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Result<T> left, in Result<T> right)
    {
        if (left is null && right is null)
            return true;

        if (left is not null && right is not null)
            return left.Equals(right);

        return false;
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Result{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Result{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(Result{T})"/> for how <see cref="Result{T}"/> equality is calculated.
    ///	</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Result<T> left, in Result<T> right) =>
        !(left == right);

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///		    A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    ///		</para>
    ///     <para>
    ///         If this is <c>Ok(<typeparamref name="T"/>)</c>, then this returns <see cref="Value"/>'s hash code.
    ///         Otherwise, if it is <c>Error</c>, it returns <see cref="Error"/>'s hash code.
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
    ///     If this is <c>Ok(<typeparamref name="T"/>)</c>, this returns <c>$"Ok({Value})"</c>.
    ///     Otherwise, it returns <see cref="Error"/>'s ToString().
    /// </remarks>
    [Pure]
    public override string ToString()
    {
        if (!_isOk)
            return _error!.ToString(includeCode: true);

        var valueString = _value!.ToString();
        // 4 accounts for "Ok(" + ")"
        var outputLength = 4 + (valueString?.Length ?? 0);
        return string.Create(
              null,
              stackalloc char[outputLength],
              $"Ok({valueString})"
            );
    }

    /// <summary>
    ///     Converts this instance to an <see cref="Option{T}"/>.
    /// </summary>
    /// <returns>An <see cref="Option{T}"/> representing this instance's <see cref="Value"/>.</returns>
    /// <remarks>
    ///     This is a one-way operation as it loses the <see cref="Error"/> component of this result.
    /// </remarks>
    [Pure]
    public Option<T> ToOption() =>
        _isOk
        ? new Option<T>(_value!)
        : Option<T>.None;

    /// <summary>
    ///     Converts this instance to an <see cref="Option{T}"/> of type <see cref="Tyne.Error"/>.
    /// </summary>
    /// <returns>An <see cref="Option{T}"/> representing this instance's <see cref="Error"/>.</returns>
    /// <remarks>
    ///     This is a one-way operation as it loses the <see cref="Value"/> component of this result.
    /// </remarks>
    [Pure]
    public Option<Error> ToErrorOption() =>
        _isOk
        ? Option<Error>.None
        : new Option<Error>(_error!);

    /// <summary>
    ///     Converts <paramref name="value"/> into an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="value">The <see cref="Result{T}"/> to convert.</param>
    /// <remarks>
    ///     This is equivalent to calling <see cref="ToOption()"/>, but is done implicitly.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<T>(Result<T> value) =>
        value is null
        ? Option<T>.None
        : value.ToOption();

    /// <summary>
    ///     Converts <paramref name="value"/> into an <see cref="Option{T}"/> of <see cref="Tyne.Error"/>.
    /// </summary>
    /// <param name="value">The <see cref="Result{T}"/> to convert.</param>
    /// <remarks>
    ///     This is equivalent to calling <see cref="ToErrorOption()"/>, but is done implicitly.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<Error>(Result<T>? value)
    {
        if (value is null)
            return Option.Some(Error.Default);

        if (value._isOk)
            return Option<Error>.None;

        return Option.Some(value.Error);
    }

    /// <summary>
    ///     Attempts to unwrap <paramref name="result"/>.
    /// </summary>
    /// <param name="result">The <see cref="Result{T}"/> to unwrap into <typeparamref name="T"/>.</param>
    /// <remarks>
    ///     This is equivalent to directly accessing <see cref="Value"/>.
    ///     You should prefer accessing this directly over casting.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator T(in Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return result.Value;
    }

    /// <summary>
    ///     Converts <paramref name="result"/> into a <see cref="Result{T}"/> of type <see cref="Unit"/>.
    /// </summary>
    /// <param name="result">The <see cref="Result{T}"/> to convert.</param>
    /// <remarks>
    ///     This is useful to discard the generic value from a result,
    ///     such as when passing it into a method that only cares about success/failure.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(result))]
    public static implicit operator Result<Unit>?(Result<T>? result)
    {
        if (result is null)
            return null;

        if (result._isOk)
            return Result.Cache.OkUnit;

        return new Result<Unit>(result.Error);
    }

    // Debugger proxy exposes _value directly.
    // This prevents issues with the debugger hitting an exception while evaluating Value or Error.
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "These members are used by the debugger.")]
    private sealed class DebuggerTypeProxy
    {
        private readonly Result<T> _result;

        public DebuggerTypeProxy(Result<T> result)
        {
            _result = result;
        }

        public bool IsOk =>
            _result._isOk;

        public T? Value =>
            _result._isOk
            ? _result._value
            : default;

        public Error? Error =>
            _result._isOk
            ? null
            : _result._error;
    }
}
