using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     An option encapsulates either <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>.
///     <list type="bullet">
///         <item>
///             In the <c>Some(<typeparamref name="T"/>)</c> case, an option holds some <see cref="Value"/>
///             (i.e. <see cref="HasValue"/> is <see langword="true"/>)
///         </item>
///         <item>
///             In the <c>None</c> case, an option is empty and holds no <typeparamref name="T"/>.
///         </item>
///     </list>
/// </summary>
/// <remarks>
///     <para>
///         The purpose of <see cref="Option{T}"/> is to provide a strong construct for handling the None case.
///         This encourages consumers to consider how to handle a missing value, rather than always assuming the happy path.
///     </para>
///     <para>
///         In functional terms, this is a polymorphic union which encapsulates either <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>.
///     </para>
///     <para>
///         See <see cref="Option"/> for how to create <see cref="Option{T}"/>s.
///     </para>
/// </remarks>
/// <typeparam name="T">The type of value this option encapsulates.</typeparam>
/// <seealso cref="OptionExtensions" />
/// <seealso cref="OptionJsonConverterFactory" />
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(Option<>.DebuggerTypeProxy))]
[JsonConverter(typeof(OptionJsonConverterFactory))]
[StructLayout(LayoutKind.Auto)]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
    private readonly bool _hasValue;
    private readonly T? _value;

    /// <summary>
    ///     Indicates whether this option is <c>Some(<typeparamref name="T"/>)</c> (i.e. has a value).
    /// </summary>
    /// <returns>
    ///     <see langword="true"/> if this option is <c>Some(<typeparamref name="T"/>)</c>;
    ///     otherwise <see langword="false"/> if it is <c>None</c>.
    /// </returns>
    [Pure]
    // This is an explicit bool rather than:
    //      HasValue => Value is not null
    // As value is never null for value types
    // It also uses an explicit backing field as it's cheaper for us to check privately
    public bool HasValue => _hasValue;

    /// <summary>
    ///     The unwrapped <typeparamref name="T"/> which this option encapsulates, if it is <c>Some(<typeparamref name="T"/>)</c>.
    /// </summary>
    /// <returns>
    ///     The <typeparamref name="T"/> which this option encapsulates.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If this option is <c>None</c>, unwrapping this will instead throw a <see cref="BadOptionException"/>.
    ///         You should ensure <see cref="HasValue"/> is <see langword="true"/> before accessing this property.
    ///     </para>
    ///     <para>
    ///         Alternatively, consider using one of <see cref="OptionExtensions"/> to access this value,
    ///         such as <see cref="OptionExtensions.Or{T}(in Option{T}, T)"/> or <see cref="OptionExtensions.Unwrap{T}(in Option{T})"/>.
    ///     </para>
    /// </remarks>
    /// <exception cref="BadOptionException">When this option is <c>None</c>.</exception>
    [Pure]
    public readonly T Value
    {
        get
        {
            if (!_hasValue)
                throw new BadOptionException(ExceptionMessages.Option_NoneHasNoValue);

            return _value!;
        }
    }

    // External callers should use Option.None() to construct a None option
    private static readonly Option<T> _none;
    internal static ref readonly Option<T> None => ref _none;

    /// <summary>
    ///     Creates an empty <see cref="Option{T}"/>.
    /// </summary>
    /// <remarks>
    ///     You should not use this constructor.
    ///     It is here to satisfy the compiler.
    ///     Instead, prefer using <see cref="Option.Some{T}(T)"/> or <see cref="Option.None{T}"/>.
    /// </remarks>
#pragma warning disable S1133 // Deprecated code should be removed. Code isn't deprecated, [Obsolete] signposts incorrect usage.
    [Obsolete($"Use {nameof(Option)}.{nameof(Option.Some)} or {nameof(Option)}.{nameof(Option.None)} to create options.", DiagnosticId = "TYN0001")]
    public Option()
    {
    }
#pragma warning restore S1133

    // External callers should use Option.Some(T) to construct a Some(T) option
    internal Option(T value)
    {
        if (value is null)
            throw new BadOptionException(ExceptionMessages.Option_SomeMustHaveValue);

        _value = value;
        _hasValue = true;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="obj"/> is an <see cref="Option{T}"/>, and if so is equal to the current instance.
    /// </summary>
    /// <param name="obj">An <see cref="object"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="obj"/> is an <see cref="Option{T}"/>,
    ///		and is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="obj"/> is an <see cref="Option{T}"/>, returns <see cref="Equals(in Option{T})"/>.
    ///     </para>
    ///     <para>
    ///         If <paramref name="obj"/> is a <typeparamref name="T"/>, returns <see cref="Equals(T?)"/>
    ///     </para>
    ///     <para>
    ///         If this instance is <c>None</c>, returns whether <paramref name="obj"/> is <see langword="null"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is Option<T> other)
            return Equals(in other);

        if (obj is T tValue)
            return Equals(tValue);

        if (obj is null && !_hasValue)
            return true;

        return false;
    }

    /// <summary>
    ///		Determines whether the specified <paramref name="other"/> is equal to the current instance of the same type.
    /// </summary>
    /// <param name="other">The other <see cref="Option{T}"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <para>
    ///         Two <see cref="Option{T}"/>s are equal if both are <c>None</c>, or if
    ///         both are <c>Some(<typeparamref name="T"/>)</c> and both <see cref="Value"/>s are equal.
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined by calling <see cref="object.Equals(object?)"/>.
    ///         If <typeparamref name="T"/> does not implement this, it may not behave as expected.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals(in Option<T> other)
    {
        if (_hasValue && other._hasValue)
            return _value!.Equals(other._value);

        return !_hasValue && !other._hasValue;
    }

    /// <summary>
    ///     Determines whether the specified <paramref name="other"/> is equal to the current instance of <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="other">The other <typeparamref name="T"/> to compare with this instance.</param>
    /// <returns>
    ///		<see langword="true"/> if the specified <paramref name="other"/> is equal to this instance; otherwise, <see langword="false"/>.
    ///	</returns>
    ///	<remarks>
    ///     <para>
    ///         Returns <see langword="true"/> if:
    ///         <list type="bullet">
    ///             <item>This instance is <c>None</c> and <paramref name="other"/> is <see langword="null"/>.</item>
    ///             <item>This instance is <c>Some(<typeparamref name="T"/>)</c> whose <see cref="Value"/> equals <paramref name="other"/>.</item>
    ///         </list>
    ///         Otherwise, returns <see langword="false"/>.
    ///     </para>
    ///     <para>
    ///         <typeparamref name="T"/> equality is determined by calling <see cref="object.Equals(object?)"/>.
    ///         If <typeparamref name="T"/> does not implement this, it may not behave as expected.
    ///     </para>
    /// </remarks>
    [Pure]
    public bool Equals(T? other)
    {
        if (_hasValue)
            return _value!.Equals(other);

        return other is null;
    }

    /// <remarks>
    ///     You should use <see cref="Equals(in Option{T})"/> instead.
    /// </remarks>
    /// <inheritdoc cref="Equals(in Option{T})" />
    // This is implemented explicitly to guide callers to using the more efficient Equals() overload instead
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool IEquatable<Option<T>>.Equals(Option<T> other) =>
        Equals(in other);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(in Option{T})"/> for how <see cref="Option{T}"/> equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Option<T> left, in Option<T> right) =>
        left.Equals(in right);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(in Option{T})"/> for how <see cref="Option{T}"/> equality is calculated.
    ///	</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Option<T> left, in Option<T> right) =>
        !left.Equals(in right);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <typeparamref name="T"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(in Option<T> left, [AllowNull] T? right) =>
        left.Equals(right);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <see cref="Option{T}"/>.</param>
    /// <param name="right">The right-hand <typeparamref name="T"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(in Option<T> left, [AllowNull] T? right) =>
        !left.Equals(right);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <typeparamref name="T"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==([AllowNull] T? left, in Option<T> right) =>
        right.Equals(left);

    /// <summary>
    ///		Determines whether the <paramref name="left"/> is not equal to the <paramref name="right"/>.
    /// </summary>
    /// <param name="left">The left-hand <typeparamref name="T"/>.</param>
    /// <param name="right">The right-hand <see cref="Option{T}"/>.</param>
    /// <returns>
    ///		<see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     <see cref="Equals(T)"/> for how equality is calculated.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=([AllowNull] T? left, in Option<T> right) =>
        !right.Equals(left);

    /// <summary>
    ///		Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    ///     <para>
    ///		    A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    ///		</para>
    ///     <para>
    ///         If this is <c>Some(<typeparamref name="T"/>)</c>, then this returns <see cref="Value"/>'s hash code.
    ///         Otherwise, if it is <c>None</c>, it returns <c>0</c>.
    ///     </para>
    /// </returns>
    [Pure]
    public override int GetHashCode() =>
        _hasValue
        ? _value!.GetHashCode()
        : 0;

    /// <summary>
    ///     Wraps <paramref name="value"/> in an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="value">The <typeparamref name="T"/> value to wrap.</param>
    /// <remarks>
    ///     This is equivalent to calling <see cref="Option.From{T}(T)"/>, but is done implicitly.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Option<T>(T? value) =>
        Option.From(value);

    /// <summary>
    ///     Attempts to unwrap <paramref name="option"/>.
    /// </summary>
    /// <param name="option">The <see cref="Option{T}"/> to unwrap into <typeparamref name="T"/>.</param>
    /// <remarks>
    ///     This is equivalent to directly accessing <see cref="Value"/>.
    ///     You should prefer accessing this directly over casting.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator T(in Option<T> option) =>
        option.Value;

    /// <summary>
    ///		Returns a <see cref="string"/> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    /// <remarks>
    ///     If this options is <c>None</c>, this returns <c>"None"</c>.
    ///     Otherwise, it returns <c>$"Some({Value})"</c>.
    /// </remarks>
    [Pure]
    public override string ToString()
    {
        if (!_hasValue)
            return "None";

        var valueString = _value!.ToString();
        // 6 accounts for "Some(" + ")"
        var outputLength = 6 + (valueString?.Length ?? 0);
        return string.Create(
              null,
              stackalloc char[outputLength],
              $"Some({valueString})"
            );
    }

    // Debugger proxy exposes _value directly.
    // This prevents issues with the debugger hitting an exception while evaluating Value.
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "These members are used by the debugger.")]
    private sealed class DebuggerTypeProxy
    {
        private readonly Option<T> _option;

        public DebuggerTypeProxy(Option<T> option)
        {
            _option = option;
        }

        public bool HasValue => _option._hasValue;

        public T? Value =>
            _option._hasValue
            ? _option._value
            : default;
    }
}
