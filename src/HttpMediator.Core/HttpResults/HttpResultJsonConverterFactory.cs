using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne.HttpMediator;

/// <summary>
///     Supports converting <see cref="HttpResult{T}"/>s to/from JSON using a factory pattern.
/// </summary>
/// <remarks>
///     While <see cref="HttpResult{T}"/>s can be serialised, it is preferred to use the dedicated reader/writer service.
/// </remarks>
/// <seealso cref="HttpResult{T}"/>
public sealed class HttpResultJsonConverterFactory : JsonConverterFactory
{
    private const BindingFlags CreateInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    /// <summary>
    ///     Determines whether the <paramref name="typeToConvert"/> can be converted to a <see cref="HttpResult{T}"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> to be checked.</param>
    /// <returns><see langword="true"/> if the <paramref name="typeToConvert"/> can be converted; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(HttpResult<>);
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
            throw new NotSupportedException($"Conversion for type \"{typeToConvert.Name}\" is not supported by this factory.");

        var converterConstructorArgs = new object[] { options };
        var typeT = typeToConvert.GetGenericArguments()[0];
        var httpResultJsonConverterType = typeof(HttpResultJsonConverter<>).MakeGenericType(typeT);
        var converterInstance = Activator.CreateInstance(
            type: httpResultJsonConverterType,
            bindingAttr: CreateInstanceBindingFlags,
            binder: null,
            args: converterConstructorArgs,
            culture: null);

        if (converterInstance is not JsonConverter converter)
            throw new InvalidOperationException($"Failed to create {nameof(JsonConverter)} instance.");

        return converter;
    }

    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Activator.")]
    [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "Constructor is used by JsonSerializer's Activator.")]
    private sealed class HttpResultJsonConverter<T> : JsonConverter<HttpResult<T>>
    {
        private sealed class ProxyType
        {
            public int StatusCode { get; set; }
            public bool IsOk { get; set; }
            public T? Value { get; set; }
            public Error? Error { get; set; }
        }

        private readonly JsonConverter<ProxyType> _proxyTypeConverter;

        public HttpResultJsonConverter(JsonSerializerOptions options)
        {
            _proxyTypeConverter = options.GetConverter<ProxyType>();
        }

        public override HttpResult<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            ArgumentNullException.ThrowIfNull(typeToConvert);
            ArgumentNullException.ThrowIfNull(options);

            var proxy = _proxyTypeConverter.Read(ref reader, typeof(ProxyType), options);
            if (proxy is null)
                return HttpResult.Error<T>(Error.Default, HttpResult.DefaultErrorStatusCode);

            var statusCode = (HttpStatusCode)proxy.StatusCode;

            if (proxy.Error is not null)
            {
                CoerceErrorStatusCode(ref statusCode);
                return HttpResult.Error<T>(proxy.Error, statusCode);
            }

            // Must be ok and have a non-null value to be OK
            if (!proxy.IsOk || proxy.Value is null)
            {
                CoerceErrorStatusCode(ref statusCode);
                return HttpResult.Error<T>(Error.Default, statusCode);
            }

            CoerceOkStatusCode(ref statusCode);
            return HttpResult.Ok(proxy.Value, statusCode);

        }

        private static void CoerceOkStatusCode(ref HttpStatusCode statusCode)
        {
            if (HttpResult.IsValidOkStatusCode(statusCode))
                return;

            statusCode = HttpResult.DefaultOkStatusCode;
        }

        private static void CoerceErrorStatusCode(ref HttpStatusCode statusCode)
        {
            if (HttpResult.IsValidErrorStatusCode(statusCode))
                return;

            statusCode = HttpResult.DefaultErrorStatusCode;
        }

        public override void Write(Utf8JsonWriter writer, HttpResult<T> value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(options);

            var resultJsonProxy = value.Match(
                ok: resultValue => new ProxyType
                {
                    StatusCode = (int)value.StatusCode,
                    IsOk = true,
                    Value = resultValue
                },
                err: resultError => new ProxyType
                {
                    StatusCode = (int)value.StatusCode,
                    IsOk = false,
                    Error = resultError
                }
            );

            _proxyTypeConverter.Write(writer, resultJsonProxy, options);
        }
    }
}
