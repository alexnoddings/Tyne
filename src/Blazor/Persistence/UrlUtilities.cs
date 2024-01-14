using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using MediatR;

namespace Tyne.Blazor.Persistence;

/// <summary>
///     Static utilities for working with URLs.
/// </summary>
internal static class UrlUtilities
{
    // DateTimes use a custom format which is less ugly and shorter than
    // the default "yyyy-MM-ddTdd%3Ahh%3Amm", and is also culture invariant.
    private const string DateTimeToStringFormat = "yyyyMMddHHmmss";

    // JSON options to use for fall-back de/serialisation
    // for types which aren't directly handled by us (e.g. user types)
    private static readonly JsonSerializerOptions FallBackJsonOptions = new(JsonSerializerDefaults.Web)
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    ///     Formats an <paramref name="obj"/> to a <see cref="string"/> using custom rules.
    /// </summary>
    /// <param name="obj">The object to format.</param>
    /// <returns></returns>
    internal static string? FormatValueToString(object? obj)
    {
        // This accepts an object rather than a generic T as some callers (e.g. bulk parameter setting) don't know the target type,
        // and maintaining two copies of the method (one object, one generic) would be a pain.
        var valueStr = obj switch
        {
            // A null value string will cause the URL to remove the parameter if it's already there
            null => null,
            string str => str.Trim(),
            // Some types are ugly when json serialised, such as chars being "quoted"
            // We handle these cases specifically for nicer looking strings
            char chr => chr.ToString(),
            Guid guid => guid.ToString("D"),
            DateTime dateTime => dateTime.ToString(DateTimeToStringFormat, provider: null),
            // Enum types are represented as strings, but IEnumerable<Enum>s are (currently) just their numeric value
            Enum enm => enm.ToString(),
            // Serialise empty collections as a null value to keep the URL tidy
            ICollection collection when collection is { Count: 0 } => null,
            _ => JsonSerializer.Serialize(obj, FallBackJsonOptions)
        };

        // Handle empty strings as null, this de-clutters URLs
        if (string.IsNullOrEmpty(valueStr))
            valueStr = null;

        // Not all collection types implement the non-generic ICollection
        // (such as HashSet<T>), so if obj is an IEnumerable which is empty,
        // set it's valueStr to be null to keep the URL tidy.
        if (obj is IEnumerable && valueStr == "[]")
            valueStr = null;

        return valueStr;
    }

    // Method looks longer than Aggressive Inlining would usually support,
    // but when inlined for a given T, the unnecessary branches can
    // be culled to result in a relatively small amount of asm.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Option<T> TryParse<T>(string valueStr)
    {
        Option<T> value;

        if (typeof(T) == typeof(string))
        {
            if (!string.IsNullOrWhiteSpace(valueStr))
                value = Option.Some((T)(object)valueStr);
            else
                value = Option.None<T>();
        }
        else if (typeof(T).IsValueType)
        {
            // A separate branch for value types helps the JIT skip unnecessary type checks for ref types
            if (typeof(T) == typeof(Unit) || typeof(T) == typeof(Unit?))
            {
                value = Option.Some((T)(object)Unit.Value);
            }
            else if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            {
                if (int.TryParse(valueStr, out var @int))
                    value = Option.Some((T)(object)@int);
                else
                    value = Option.None<T>();
            }
            else if (typeof(T) == typeof(Guid) || typeof(T) == typeof(Guid?))
            {
                if (Guid.TryParse(valueStr, out var guid))
                    value = Option.Some((T)(object)guid);
                else
                    value = Option.None<T>();
            }
            else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            {
                if (DateTime.TryParseExact(valueStr, DateTimeToStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                    value = Option.Some((T)(object)dateTime);
                else
                    value = Option.None<T>();
            }
            else if (IsTargetTypeAnEnum<T>(out var enumType))
            {
                if (Enum.TryParse(enumType, valueStr, out var @enum))
                    value = Option.Some((T)@enum);
                else
                    value = Option.None<T>();
            }
            else if (typeof(T) == typeof(char) || typeof(T) == typeof(char?))
            {
                if (valueStr.Length == 1)
                    value = Option.Some((T)(object)valueStr[0]);
                else
                    value = Option.None<T>();
            }
            else
            {
                value = Deserialise(valueStr);
            }
        }
        else
        {
            value = Deserialise(valueStr);
        }

        return value;

        static Option<T> Deserialise(string valueStr)
        {
            try
            {
                var val = JsonSerializer.Deserialize<T?>(valueStr, FallBackJsonOptions);
                return Option.From(val);
            }
            catch (JsonException)
            {
                return Option.None<T>();
            }
        }

        // Calling Enum.TryParse() with a Nullable<TEnum> will throw an exception,
        // so as part of figuring out if TActual is an Enum, we also find TEnum
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsTargetTypeAnEnum<TActual>([NotNullWhen(true)] out Type? enumType)
        {
            var actual = typeof(TActual);
            if (actual.IsEnum)
            {
                enumType = actual;
                return true;
            }

            if (actual.IsValueType)
            {
                var nullableUnderlyingType = Nullable.GetUnderlyingType(actual);
                if (nullableUnderlyingType?.IsEnum is true)
                {
                    enumType = nullableUnderlyingType;
                    return true;
                }
            }

            enumType = null;
            return false;
        }
    }
}
