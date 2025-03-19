using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     Supports converting <see cref="Option{T}"/>s to/from JSON using a factory pattern.
/// </summary>
/// <seealso cref="Option{T}"/>
public sealed class OptionJsonConverterFactory : JsonConverterFactory
{
    private const BindingFlags CreateInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    /// <summary>
    ///     Determines whether the <paramref name="typeToConvert"/> can be converted to an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> to be checked.</param>
    /// <returns><see langword="true"/> if the <paramref name="typeToConvert"/> can be converted; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);
    }

    /// <summary>
    ///     Create a <see cref="JsonConverter"/> for the provided <paramref name="typeToConvert"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> being converted.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> being used.</param>
    /// <returns>
    ///     An instance of a <see cref="JsonConverter{T}"/> where T is compatible with <paramref name="typeToConvert"/>.
    /// </returns>
    /// <remarks>
    ///     While this internally verifies <see cref="CanConvert(Type)"/>, this should be checked first by the caller.
    /// </remarks>
    /// <exception cref="ArgumentNullException">When <paramref name="typeToConvert"/> or <paramref name="options"/> are <see langword="null"/>.</exception>
    /// <exception cref="NotSupportedException">If <see cref="CanConvert(Type)"/> returns <see langword="false"/> for <paramref name="typeToConvert"/>.</exception>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        if (!CanConvert(typeToConvert))
            throw new NotSupportedException(ExceptionMessages.JsonConversionForTypeNotSupported(typeToConvert));

        var converterConstructorArgs = new object[] { options };
        var typeT = typeToConvert.GetGenericArguments()[0];
        var optionJsonConverterType = typeof(OptionJsonConverter<>).MakeGenericType(typeT);
        var converterInstance = Activator.CreateInstance(
            type: optionJsonConverterType,
            bindingAttr: CreateInstanceBindingFlags,
            binder: null,
            args: converterConstructorArgs,
            culture: null);

        if (converterInstance is not JsonConverter converter)
            throw new InvalidOperationException(ExceptionMessages.JsonConverterFactoryCouldNotCreateConverter);

        return converter;
    }

    [SuppressMessage("Performance", "CA1812: Avoid un-instantiated internal classes", Justification = "Class is instantiated by Activator.")]
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "Constructor is used by Activator.")]
    private sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
    {
        private readonly JsonConverter<T> _optionTConverter;

        public OptionJsonConverter(JsonSerializerOptions options)
        {
            _optionTConverter = options.GetConverter<T>();
        }

        public override Option<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(typeToConvert);
            ArgumentNullException.ThrowIfNull(options);

            if (reader.TokenType is JsonTokenType.Null)
                return Option<T>.None;

            var value = _optionTConverter.Read(ref reader, typeof(T), options);
            if (value is null)
                return Option<T>.None;

            return Option.Some(value);
        }

        public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(options);

            if (value.HasValue)
                _optionTConverter.Write(writer, value.Value, options);
            else
                writer.WriteNullValue();
        }
    }
}
