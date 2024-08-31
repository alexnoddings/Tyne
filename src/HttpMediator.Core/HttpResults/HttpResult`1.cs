using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Tyne.HttpMediator;

/// <summary>
///     An extension of <see cref="HttpResult{T}"/> which incorporates a <see cref="StatusCode"/>.
/// </summary>
/// <typeparam name="T">The type of value this HttpResult encapsulates.</typeparam>
/// <remarks>
///     See the documentation for <see cref="Result{T}"/>.
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(HttpResult<>.DebuggerTypeProxy))]
[JsonConverter(typeof(HttpResultJsonConverterFactory))]
public class HttpResult<T> : Result<T>, IEquatable<HttpResult<T>>
{
    /// <summary>
    ///     The <see cref="HttpStatusCode"/> which represents this result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This is separate to the <see cref="Error.Code"/>.
    ///         The error code is more granular, differentiating individual errors
    ///         (e.g. validation error or invalid state) whereas the <see cref="StatusCode"/>
    ///         is used to identify which status code to use in HTTP responses.
    ///     </para>
    ///     <para>
    ///         An <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/>'s status code must be in the range <c>200 &lt;= statusCode &lt;= 299</c>.
    ///         This is enforced by the constructor.
    ///     </para>
    ///     <para>
    ///         An <c>Error</c> <see cref="HttpResult{T}"/>'s status code must be in the range <c>400 &lt;= statusCode &lt;= 599</c>.
    ///         This is enforced by the constructor.
    ///     </para>
    /// </remarks>
    [Pure]
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    ///     Creates an <c>Ok(<typeparamref name="T"/>)</c> <see cref="HttpResult{T}"/>.
    /// </summary>
    /// <param name="value">The <see cref="Result{T}.Value"/>. Cannot be <see langword="null"/>.</param>
    /// <param name="statusCode">The <see cref="HttpResult{T}.StatusCode"/>.</param>
    /// <exception cref="BadResultException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="statusCode"/> is not a valid Ok status code.</exception>
    /// <remarks>
    ///     This constructor is only available to inheritors.
    /// </remarks>
    protected internal HttpResult(T value, HttpStatusCode statusCode) : base(value)
    {
        EnsureIsValidOkStatusCode(statusCode);
        StatusCode = statusCode;
    }

    /// <summary>
    ///     Creates an <c>Error</c> <see cref="HttpResult{T}"/>.
    /// </summary>
    /// <param name="error">The <see cref="Result{T}.Error"/>. Cannot be <see langword="null"/>.</param>
    /// <param name="statusCode">The <see cref="HttpResult{T}.StatusCode"/>.</param>
    /// <exception cref="BadResultException">When <paramref name="error"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">When <paramref name="statusCode"/> is not a valid Error status code.</exception>
    /// <remarks>
    ///     This constructor is only available to inheritors.
    /// </remarks>
    protected internal HttpResult(Error error, HttpStatusCode statusCode) : base(error)
    {
        EnsureIsValidErrorStatusCode(statusCode);
        StatusCode = statusCode;
    }

    [StackTraceHidden]
    internal static void EnsureIsValidOkStatusCode(HttpStatusCode statusCode)
    {
        if (HttpResult.IsValidOkStatusCode(statusCode))
            return;

        var argOutOfRangeException = new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "Ok status codes must be in the range 200 <= c < 299.");
        throw new BadResultException(CoreExceptionMessages.HttpResult_OkStatusCodeOutOfRange, argOutOfRangeException);
    }

    [StackTraceHidden]
    internal static void EnsureIsValidErrorStatusCode(HttpStatusCode statusCode)
    {
        if (HttpResult.IsValidErrorStatusCode(statusCode))
            return;

        var argOutOfRangeException = new ArgumentOutOfRangeException(nameof(statusCode), statusCode, "Error status codes must be in the range 400 <= c < 599.");
        throw new BadResultException(CoreExceptionMessages.HttpResult_ErrorStatusCodeOutOfRange, argOutOfRangeException);
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is a <see cref="HttpResult{T}"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is a <see cref="HttpResult{T}"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is HttpResult<T> other)
            return Equals(other);

        return false;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="HttpResult{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Two <see cref="HttpResult{T}"/>s are equal if their <see cref="StatusCode"/>s are equal,
    ///         and they pass <see cref="Result{T}"/>'s equality check.
    ///     </para>
    /// </remarks>
    [Pure]
    public virtual bool Equals([NotNullWhen(true)] HttpResult<T>? other)
    {
        if (other is null)
            return false;

        return StatusCode == other.StatusCode && Equals(other as Result<T>);
    }

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="HttpResult{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="HttpResult{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(HttpResult{T})"/> for how <see cref="HttpResult{T}"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in HttpResult<T> left, in HttpResult<T> right)
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
    /// <param name="left">The left-hand <see cref="HttpResult{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="HttpResult{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(HttpResult{T})"/> for how <see cref="HttpResult{T}"/> equality is calculated.
    ///	</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in HttpResult<T> left, in HttpResult<T> right) =>
        !(left == right);

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///		    A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    ///		</para>
    ///     <para>
    ///         This returns the base <see cref="Result{T}.GetHashCode()"/> combined with <see cref="StatusCode"/>.
    ///     </para>
    /// </returns>
    [Pure]
    public override int GetHashCode() =>
        HashCode.Combine(base.GetHashCode(), StatusCode);

    /// <summary>
    ///		Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <remarks>
    ///     Returns the <see cref="StatusCode"/> followed by the base <see cref="Result{T}.ToString"/>.
    /// </remarks>
    [Pure]
    public override string ToString()
    {
        var baseString = base.ToString();
        // 5 accounts for "XXX: "
        var outputLength = 5 + baseString.Length;

        return string.Create(
            null,
            stackalloc char[outputLength],
            $"{(int)StatusCode:000}: {baseString}"
        );
    }

    /// <summary>
    ///     Converts <paramref name="result"/> into a <see cref="HttpResult{T}"/> of type <see cref="Unit"/>.
    /// </summary>
    /// <param name="result">The <see cref="HttpResult{T}"/> to convert.</param>
    /// <remarks>
    ///     This is useful to discard the generic value from a result,
    ///     such as when passing it into a method that only cares about success/failure.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull(nameof(result))]
    public static implicit operator HttpResult<Unit>?(HttpResult<T>? result)
    {
        if (result is null)
            return null;

        if (result.IsOk)
            return new(Unit.Value, result.StatusCode);

        return new HttpResult<Unit>(result.Error, result.StatusCode);
    }

    // Debugger proxy exposes base values.
    // This prevents issues with the debugger hitting an exception while evaluating Value or Error.
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "These members are used by the debugger.")]
    private sealed class DebuggerTypeProxy
    {
        private readonly HttpResult<T> _result;

        public DebuggerTypeProxy(HttpResult<T> httpResult)
        {
            _result = httpResult;
        }

        public bool IsOk =>
            _result.IsOk;

        public HttpStatusCode StatusCode =>
            _result.StatusCode;

        public T? Value =>
            IsOk
            ? _result.Value
            : default;

        public Error? Error =>
            IsOk
            ? null
            : _result.Error;
    }
}
