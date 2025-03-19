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

        // Must be generic to be Option<,>
        // Must be constructed (eg Option<int>) rather than unconstructed (eg Option<>) so the type params are concrete
        return typeToConvert is { IsGenericType: true, IsConstructedGenericType: true }
               && typeToConvert.GetGenericTypeDefinition() == typeof(Option<>);
    }

    /// <summary>
    ///     Creates a <see cref="JsonConverter"/> for the provided <paramref name="typeToConvert"/>.
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
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        if (!CanConvert(typeToConvert))
            throw new NotSupportedException(ExceptionMessages.JsonConverter_ConversionForTypeNotSupported(typeToConvert));

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
            throw new InvalidOperationException(ExceptionMessages.JsonConverter_FactoryCouldNotCreateConverter);

        return converter;
    }
}
