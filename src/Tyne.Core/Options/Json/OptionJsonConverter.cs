using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

internal sealed class OptionJsonConverter<T> : JsonConverter<Option<T>>
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
            return Option.Cache<T>.None;

        var value = _optionTConverter.Read(ref reader, typeof(T), options);
        if (value is null)
            return Option.Cache<T>.None;

        return Option.Some(value);
    }

    public override void Write(Utf8JsonWriter writer, Option<T> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(options);

        if (value.TryUnwrap(out var inner))
            _optionTConverter.Write(writer, inner, options);
        else
            writer.WriteNullValue();
    }
}
