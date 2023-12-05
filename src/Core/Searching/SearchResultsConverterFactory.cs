using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Tyne.Searching;

/// <summary>
///     Supports converting <see cref="SearchResults{T}"/>s by using a factory pattern.
/// </summary>
public sealed class SearchResultsConverterFactory : JsonConverterFactory
{
    private const BindingFlags CreateInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    /// <summary>
    ///     Determines whether the <paramref name="typeToConvert"/> can be converted to a <see cref="SearchResults{T}"/>.
    /// </summary>
    /// <param name="typeToConvert">The <see cref="Type"/> to be checked.</param>
    /// <returns><see langword="true"/> if the <paramref name="typeToConvert"/> can be converted, otherwise <see langword="true"/>.</returns>
    public override bool CanConvert(Type typeToConvert)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);

        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(SearchResults<>);
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
    /// <exception cref="NotSupportedException">If <see cref="CanConvert(Type)"/> returns false for <paramref name="typeToConvert"/>.</exception>
    [SuppressMessage("Major Code Smell", "S2589: Boolean expressions should not be gratuitous", Justification = "False positive.")]
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);

        if (!CanConvert(typeToConvert))
            throw new NotSupportedException($"Conversion for type \"{typeToConvert?.Name}\" is not supported by this factory.");

        var converterConstructorArgs = new object[] { options };
        var typeT = typeToConvert.GetGenericArguments()[0];
        var resultJsonConverterType = typeof(SearchResultsConverter<>).MakeGenericType(typeT);
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

    private sealed class SearchResultsJsonProxyType<T>
    {
        public int TotalCount { get; set; }
        public List<T> Values { get; set; } = new();
    }

    [SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes", Justification = "Class is instantiated by Activator.CreateInstance.")]
    private sealed class SearchResultsConverter<T> : JsonConverter<SearchResults<T>>
    {
        private readonly JsonConverter<SearchResultsJsonProxyType<T>> _proxyTypeConverter;

        [SuppressMessage("Major Code Smell", "S1144: Unused private types or members should be removed", Justification = "Class is instantiated by Activator.CreateInstance.")]
        public SearchResultsConverter(JsonSerializerOptions options)
        {
            _proxyTypeConverter = options.GetConverter<SearchResultsJsonProxyType<T>>();
        }

        [SuppressMessage("Major Code Smell", "S1168: Empty arrays and collections should be returned instead of null", Justification = "JsonConverter should return null if the type cannot be read.")]
        public override SearchResults<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var searchResultsJsonProxy = _proxyTypeConverter.Read(ref reader, typeof(SearchResultsJsonProxyType<T>), options);
            if (searchResultsJsonProxy is null)
                return null;

            return new SearchResults<T>(searchResultsJsonProxy.Values, searchResultsJsonProxy.TotalCount);
        }

        public override void Write(Utf8JsonWriter writer, SearchResults<T> value, JsonSerializerOptions options)
        {
            var searchResultsJsonProxy = new SearchResultsJsonProxyType<T>
            {
                TotalCount = value.TotalCount,
                Values = value.ToList()
            };
            _proxyTypeConverter.Write(writer, searchResultsJsonProxy, options);
        }
    }
}
