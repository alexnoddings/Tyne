using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne;

/// <summary>
///     Supports converting <see cref="Result{T}"/>s to/from JSON using a factory pattern.
/// </summary>
/// <seealso cref="Result{T}"/>
public sealed class ResultJsonConverterFactory : JsonConverterFactory
{
    private const BindingFlags CreateInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    /// <summary>
    ///     Determines whether the <paramref name="typeToConvert"/> can be converted to a <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> to be checked.</param>
    /// <returns><see langword="true"/> if the <paramref name="typeToConvert"/> can be converted; otherwise, <see langword="false"/>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
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
        var resultJsonConverterType = typeof(ResultJsonConverter<>).MakeGenericType(typeT);
        var converterInstance = Activator.CreateInstance(
            type: resultJsonConverterType,
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
    private sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
    {
        private sealed class ResultJsonProxyType
        {
            public bool IsOk { get; set; }
            public T? Value { get; set; }
            public Error? Error { get; set; }
        }

        private readonly JsonConverter<ResultJsonProxyType> _resultJsonProxyTypeConverter;

        public ResultJsonConverter(JsonSerializerOptions options)
        {
            _resultJsonProxyTypeConverter = options.GetConverter<ResultJsonProxyType>();
        }

        public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(typeToConvert);
            ArgumentNullException.ThrowIfNull(options);

            var resultJsonProxy = _resultJsonProxyTypeConverter.Read(ref reader, typeof(ResultJsonProxyType), options);
            if (resultJsonProxy is null)
                return Result.Error<T>(Error.Default);

            if (resultJsonProxy.Error is not null)
                return Result.Error<T>(resultJsonProxy.Error);

            // Must be ok and have a non-null value to be OK
            if (!resultJsonProxy.IsOk || resultJsonProxy.Value is null)
                return Result.Error<T>(Error.Default);

            return Result.Ok(resultJsonProxy.Value);
        }

        public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(writer);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(options);

            var resultJsonProxy = value.Match(
                ok: resultValue => new ResultJsonProxyType
                {
                    IsOk = true,
                    Value = resultValue
                },
                err: resultError => new ResultJsonProxyType
                {
                    IsOk = false,
                    Error = resultError
                }
            );

            _resultJsonProxyTypeConverter.Write(writer, resultJsonProxy, options);
        }
    }
}
