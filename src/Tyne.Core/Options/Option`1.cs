using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Tyne.Utilities;

namespace Tyne;

/// <summary>
///     An option encapsulates either <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>.
/// </summary>
/// <remarks>
///     <para>
///         The purpose of <see cref="Option{T}"/> is to provide a strong construct for handling the None case.
///         This encourages consumers to consider how to handle a missing value, rather than assuming the happy path.
///     </para>
///     <para>
///         In functional terms, this is a polymorphic union which encapsulates either <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>.
///     </para>
///     <para>
///         See <see cref="Option"/> for how to create <see cref="Option{T}"/>s.
///     </para>
///     <para>
///         <c>Some(42)</c> is considered equal to <c>Some(42)</c> and <c>42</c>.
///         <c>None&lt;int?&gt;</c> is considered equal to <c>None&lt;int?&gt;</c> and <see langword="null"/>.
///     </para>
/// </remarks>
/// <typeparam name="T">The type of value this option encapsulates.</typeparam>
/// <seealso cref="OptionExtensions" />
/// <seealso cref="OptionJsonConverterFactory" />
[DebuggerDisplay("{ToString(),nq}")]
[DebuggerTypeProxy(typeof(Option<>.DebuggerTypeProxy))]
[JsonConverter(typeof(OptionJsonConverterFactory))]
[StructLayout(LayoutKind.Auto)]
public readonly partial struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
    private readonly bool _hasValue;
    private readonly T? _value;

    /// <summary>
    ///     Creates an empty <see cref="Option{T}"/>.
    /// </summary>
    /// <remarks>
    ///     You should not use this constructor, it is here to satisfy the compiler.
    ///     Instead, prefer using <see cref="Option.Some{T}(T)"/> or <see cref="Option.None{T}"/>.
    /// </remarks>
    [SuppressMessage(
        "Info Code Smell",
        "S1133:Deprecated code should be removed",
        Justification = "Deprecation is used to nudge consumers away from using the constructor"
    )]
    [Obsolete($"Use {nameof(Option)}.{nameof(Option.Some)} or {nameof(Option)}.{nameof(Option.None)} to create options.", DiagnosticId = "TYNE001")]
    public Option()
    {
    }

    // External callers should use Option.Some(T) to construct a Some(T) option
    internal Option([DisallowNull] T value)
    {
        _hasValue = true;
        _value = value;
    }

    [Pure]
    public bool TryUnwrap([NotNullWhen(true)] out T? value)
    {
        if (_hasValue)
        {
            value = _value!;
            return true;
        }

        value = default;
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
    ///         If this is <c>Some(<typeparamref name="T"/>)</c>, then this returns the <typeparamref name="T"/> value's hash code.
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
    public static implicit operator Option<T>(in T? value) =>
        Option.From(value);

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

        return TyneToStringHelpers.CreateStringFor("Some", _value!);
    }

    // Debugger proxy exposes _hasValue/_value more cleanly.
    [ExcludeFromCodeCoverage]
    [SuppressMessage(
        "Major Code Smell",
        "S1144: Unused private types or members should be removed",
        Justification = "These members are used by the debugger."
    )]
    private sealed class DebuggerTypeProxy
    {
        public bool HasValue { get; }
        public T? Value { get; }

        public DebuggerTypeProxy(Option<T> option)
        {
            HasValue = option._hasValue;
            Value = option._value;
        }
    }
}
