using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

internal sealed class ResultJsonConverter<T, TE> : JsonConverter<Result<T, TE>>
{
    private sealed class Proxy
    {
        public const string OkType = "ok";
        public const string ErrorType = "error";

        [JsonPropertyName("$")] public string? Type { get; set; }
        public T? Value { get; set; }
        public TE? Error { get; set; }
    }

    private readonly JsonConverter<Proxy> _proxyTypeConverter;

    public ResultJsonConverter(JsonSerializerOptions options)
    {
        _proxyTypeConverter = options.GetConverter<Proxy>();
    }

    [SuppressMessage(
        "Minor Code Smell",
        "S2219: Runtime type checking should be simplified.",
        Justification = "False positive, wonky analyser."
    )]
    public override Result<T, TE>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        var proxy = _proxyTypeConverter.Read(ref reader, typeof(Proxy), options);
        if (proxy is null)
            return null;

        switch (proxy.Type)
        {
            case Proxy.OkType:
            {
                var value = proxy.Value;
                if (value is null)
                    throw new JsonException(ExceptionMessages.Result_JsonConverter_OkButNoValue);

                return Result.Ok<T, TE>(value);
            }
            case Proxy.ErrorType:
            {
                var error = proxy.Error;
                if (error is null)
                    throw new JsonException(ExceptionMessages.Result_JsonConverter_ErrorButNoError);

                return Result.Error<T, TE>(error);
            }
            case null or "":
                throw new JsonException(ExceptionMessages.Result_JsonConverter_NoResultType);
            default:
                throw new JsonException(ExceptionMessages.Result_JsonConverter_InvalidType(proxy.Type));
        }
    }

    public override void Write(Utf8JsonWriter writer, Result<T, TE> value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(options);

        Proxy proxy;
        if (value.TryUnwrap(out var resultValue, out var resultError))
            proxy = new() { Type = Proxy.OkType, Value = resultValue };
        else
            proxy = new() { Type = Proxy.ErrorType, Error = resultError };

        _proxyTypeConverter.Write(writer, proxy, options);
    }
}
