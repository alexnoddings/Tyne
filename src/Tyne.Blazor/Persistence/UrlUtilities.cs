using System.Buffers.Text;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

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
    private static readonly JsonSerializerOptions _fallBackJsonOptions = new(JsonSerializerDefaults.Web)
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
            Guid guid => CompactGuid(in guid),
            DateTime dateTime => dateTime.ToString(DateTimeToStringFormat, provider: null),
            // Enum types are represented as strings, but IEnumerable<Enum>s are (currently) just their numeric value
            Enum enm => enm.ToString(),
            // Serialise empty collections as a null value to keep the URL tidy
            ICollection collection when collection is { Count: 0 } => null,
            _ => JsonSerializer.Serialize(obj, _fallBackJsonOptions)
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
    [SuppressMessage("Style", "IDE0045: Convert to conditional expression.", Justification = "Some conditional expressions are less read-able than a plain ol' if/else.")]
    internal static Option<T> TryParse<T>(string valueStr)
    {
        Option<T> value;

        if (typeof(T) == typeof(string))
        {
            value = string.IsNullOrWhiteSpace(valueStr)
                ? Option.None<T>()
                : Option.Some((T)(object)valueStr);
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
                value = int.TryParse(valueStr, out var @int)
                    ? Option.Some((T)(object)@int)
                    : Option.None<T>();
            }
            else if (typeof(T) == typeof(Guid) || typeof(T) == typeof(Guid?))
            {
                // First try de-compacting the GUID, and fall back to regular GUID parsing if that fails
                if (TryDecompactGuid(valueStr, out var guid))
                    value = Option.Some((T)(object)guid);
                else if (Guid.TryParse(valueStr, out guid))
                    value = Option.Some((T)(object)guid);
                else
                    value = Option.None<T>();
            }
            else if (typeof(T) == typeof(DateTime) || typeof(T) == typeof(DateTime?))
            {
                value = DateTime.TryParseExact(valueStr, DateTimeToStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime)
                    ? Option.Some((T)(object)dateTime)
                    : Option.None<T>();
            }
            else if (IsTargetTypeAnEnum<T>(out var enumType))
            {
                value = Enum.TryParse(enumType, valueStr, out var @enum)
                    ? Option.Some((T)@enum)
                    : Option.None<T>();
            }
            else if (typeof(T) == typeof(char) || typeof(T) == typeof(char?))
            {
                value = valueStr.Length == 1
                    ? Option.Some((T)(object)valueStr[0])
                    : Option.None<T>();
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
                var val = JsonSerializer.Deserialize<T?>(valueStr, _fallBackJsonOptions);
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

    private const byte ForwardSlashByte = (byte)'/';
    private const char ForwardSlashReplacementChar = '@';

    private const byte PlusByte = (byte)'+';
    private const char PlusReplacementChar = '%';

    // GUIDs are compacted into a special Base64 form which cuts down
    // their footprint in URLs from 32 chars to 22 chars (~31% smaller).
    // This helps keep URLs shorter for pages (e.g. tables) which persist a few GUIDs.
    private static string CompactGuid(
#if NET8_0_OR_GREATER
        in Guid guid
#else
        ref Guid guid
#endif
    )
    {
        // Short circuit empty GUIDs
        if (guid == Guid.Empty)
            return "AAAAAAAAAAAAAAAAAAAAAA";

        // Guids are always 16 bytes (128 bit)
        Span<byte> guidBytes = stackalloc byte[16];
        // Our base64 encoding is always 22 chars + 2 padding chars
        Span<byte> encodedBytes = stackalloc byte[24];

        // Marshal the GUID struct into a byte span
#if NET8_0_OR_GREATER
        MemoryMarshal.TryWrite(guidBytes, in guid);
#else
        MemoryMarshal.TryWrite(guidBytes, ref guid);
#endif

        // Converts the GUID bytes to Base64 bytes
        Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

        // The output chars (we disregard the last 2 padding chars)
        Span<char> chars = stackalloc char[22];

        // Convert our bytes to chars, then replace special chars (/ and +)
        // with our own symbols which are more friendly for URL encoding
        for (var i = 0; i < 22; i++)
        {
            chars[i] = encodedBytes[i] switch
            {
                // Replace special chars
                ForwardSlashByte => ForwardSlashReplacementChar,
                PlusByte => PlusReplacementChar,
                // Cast other bytes to their char counterpart
                _ => (char)encodedBytes[i],
            };
        }

        // Create a string from the char span
        return new string(chars);
    }

    // The inverse of GUID compacting
    private static bool TryDecompactGuid(string str, out Guid guid)
    {
        // Compacted GUIDs should always be 22 chars long
        if (str.Length != 22)
        {
            guid = Guid.Empty;
            return false;
        }

        // Short circuit for empty GUIDs
        if (str == "AAAAAAAAAAAAAAAAAAAAAA")
        {
            guid = Guid.Empty;
            return true;
        }

        // Ensure that it is a valid compressed GUID
        for (var i = 0; i < 22; i++)
        {
            var c = str[i];
            // Chars must be a-z
            if (c is >= 'a' and <= 'z')
                continue;
            // Or A-Z
            if (c is >= 'A' and <= 'Z')
                continue;
            // Or 0-9
            if (c is >= '0' and <= '9')
                continue;
            // Or one of our replacement chars
            if (c is ForwardSlashReplacementChar or PlusReplacementChar)
                continue;

            // If it isn't one of those, then it isn't valid
            // All 22 char combinations of the above chars are valid
            guid = Guid.Empty;
            return false;
        }

        // Guids are always 16 bytes (128 bit)
        Span<byte> guidBytes = stackalloc byte[16];
        // Our base64 encoding is always 22 chars + 2 padding chars
        Span<byte> decodedBytes = stackalloc byte[24];

        // Convert our chars to bytes, and swap back special chars
        for (var i = 0; i < 22; i++)
        {
            decodedBytes[i] = str[i] switch
            {
                // Replace special chars
                ForwardSlashReplacementChar => ForwardSlashByte,
                PlusReplacementChar => PlusByte,
                // Cast other char to their byte counterpart
                _ => (byte)str[i],
            };
        }

        // Final two bytes are always the same padding
        decodedBytes[22] = 61;
        decodedBytes[23] = 61;

        // Converts the Base64 bytes to GUID bytes
        _ = Base64.DecodeFromUtf8(decodedBytes, guidBytes, out _, out _);

        // Marshal the bytes into the GUID struct
        _ = MemoryMarshal.TryRead(guidBytes, out guid);

        return true;
    }
}
