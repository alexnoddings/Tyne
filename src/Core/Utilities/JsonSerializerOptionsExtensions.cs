namespace System.Text.Json.Serialization;

/// <summary>
///     Extensions for working with <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    ///     Loads a <see cref="JsonConverter{T}"/> for <typeparamref name="T"/> from <paramref name="options"/>.
    ///     Throws an <see cref="InvalidOperationException"/> if one is not found.
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
    /// <param name="options">A <see cref="JsonSerializerOptions"/>.</param>
    /// <returns>
    ///     A <see cref="JsonConverter{T}"/> for <typeparamref name="T"/> from <paramref name="options"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="options"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">When no <see cref="JsonConverter{T}"/> for <typeparamref name="T"/> is found in <paramref name="options"/>.</exception>
    public static JsonConverter<T> GetConverter<T>(this JsonSerializerOptions options) =>
        options.TryGetConverter<T>()
        ?? throw new InvalidOperationException($"{nameof(options)} did not contain a valid {nameof(JsonConverter)}<{typeof(T).Name}>.");

    /// <summary>
    ///     Tries to load a <see cref="JsonConverter{T}"/> for <typeparamref name="T"/> from <paramref name="options"/>.
    ///     Returns <see langword="null"/> if one is not found.
    /// </summary>
    /// <typeparam name="T">The type of object or value handled by the converter.</typeparam>
    /// <param name="options">A <see cref="JsonSerializerOptions"/>.</param>
    /// <returns>
    ///     A <see cref="JsonConverter{T}"/> for <typeparamref name="T"/> if one was found in <paramref name="options"/>.
    ///     Otherwise, <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">When <paramref name="options"/> is <see langword="null"/>.</exception>
    public static JsonConverter<T>? TryGetConverter<T>(this JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var jsonConverter = options.GetConverter(typeof(T));
        if (jsonConverter is not JsonConverter<T> jsonConverterT)
            return null;

        return jsonConverterT;
    }
}
