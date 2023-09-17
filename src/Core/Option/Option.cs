using System.Diagnostics.Contracts;

namespace Tyne;

/// <summary>
///     Static methods for creating <see cref="Option{T}"/>s.
/// </summary>
public static class Option
{
    /// <summary>
    ///     Creates a <c>None</c> <see cref="Option{T}"/> (i.e. <see cref="Option{T}.HasValue"/> is <see langword="false"/>).
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <returns>A <see langword="ref"/> <see langword="readonly"/> <c>None</c> <see cref="Option{T}"/>.</returns>
    [Pure]
    public static ref readonly Option<T> None<T>() =>
        ref Option<T>.None;

    /// <summary>
    ///     Creates a <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> using <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to wrap.</param>
    /// <returns>A <c>Some(<typeparamref name="T"/>)</c> <see cref="Option{T}"/> which wraps <paramref name="value"/>.</returns>
    /// <remarks>
    ///     A <see cref="BadOptionException"/> will be thrown if <paramref name="value"/> is <see langword="null"/>.
    ///     If you are unsure if <paramref name="value"/> is <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>,
    ///     consider calling <see cref="From{T}(T)"/> instead.
    /// </remarks>
    /// <exception cref="BadOptionException">When <paramref name="value"/> is <see langword="null"/>.</exception>
    [Pure]
    public static Option<T> Some<T>(T value)
    {
        if (value is null)
            throw new BadOptionException(ExceptionMessages.Option_SomeMustHaveValue);

        return new(value);
    }

    /// <summary>
    ///     Creates an <see cref="Option{T}"/> from <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value the option encapsulates.</typeparam>
    /// <param name="value">The <typeparamref name="T"/> to potentially wrap.</param>
    /// <returns>An <see cref="Option{T}"/> which may be <c>Some(<typeparamref name="T"/>)</c> or <c>None</c>, depending on <paramref name="value"/>.</returns>
    /// <remarks>
    ///     <para>
    ///         If <paramref name="value"/> is <see langword="null"/>, this will return <see cref="None{T}"/>.
    ///         Otherwise, this will return <see cref="Some{T}(T)"/>.
    ///     </para>
    ///     <para>
    ///         Bear in mind that value-typed <typeparamref name="T"/>s will always return <c>Some(<typeparamref name="T"/>)</c> as <typeparamref name="T"/> cannot be <see langword="null"/>.
    ///     </para>
    /// </remarks>
    [Pure]
    public static Option<T> From<T>(T? value) =>
        value is null
        ? Option<T>.None
        : new(value);
}
