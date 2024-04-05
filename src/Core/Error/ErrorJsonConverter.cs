using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     Supports converting <see cref="Error"/>s to/from JSON.
/// </summary>
/// <seealso cref="Error"/>
public sealed class ErrorJsonConverter : JsonConverter<Error>
{
    [SuppressMessage("Minor Code Smell", "S3459: Unassigned members should be removed", Justification = "Assignment is done by the json converter.")]
    private sealed class ErrorJsonProxyType
    {
        public string? Code { get; set; }
        public string? Message { get; set; }
    }

    private readonly JsonConverter<ErrorJsonProxyType>? _errorJsonProxyTypeConverter;

    public ErrorJsonConverter()
    {
    }

    public ErrorJsonConverter(JsonSerializerOptions options)
    {
        _errorJsonProxyTypeConverter = options.GetConverter<ErrorJsonProxyType>();
    }

    public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        var errorJsonProxyConverter =
            _errorJsonProxyTypeConverter
            ?? options.GetConverter<ErrorJsonProxyType>();

        var errorJsonProxy = errorJsonProxyConverter.Read(ref reader, typeof(ErrorJsonProxyType), options);
        if (errorJsonProxy is null)
            return Error.Default;

        return new Error(errorJsonProxy.Code, errorJsonProxy.Message, null);
    }

    public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);

        writer.WriteStartObject();

        writer.WriteString(nameof(ErrorJsonProxyType.Code), value.Code);
        writer.WriteString(nameof(ErrorJsonProxyType.Message), value.Message);
        // CausedBy is intentionally never written

        writer.WriteEndObject();
    }
}
