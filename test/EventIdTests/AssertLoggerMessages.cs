using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Tyne;

internal static class AssertLoggerMessages
{
    private const BindingFlags MethodsBindingFlags =
        // Logger methods can be public/internal/private
        BindingFlags.Public
        | BindingFlags.NonPublic
        // But they always need to be static
        | BindingFlags.Static;

    /// <summary>
    ///     Checks if any methods in <paramref name="assembly"/> are marked with <see cref="LoggerMessageAttribute"/>,
    ///     and if so ensures that <see cref="LoggerMessageAttribute.EventId"/> is in the interval <c>[<paramref name="min"/>, <paramref name="max"/>]</c>
    ///     (i.e. the bounds are inclusive).
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to check.</param>
    /// <param name="range">The <see cref="Range"/> of allowed <see cref="LoggerMessageAttribute.EventId"/>s.</param>
    /// <remarks>
    ///     <paramref name="range"/> does not support from-end starts, but does support from-end ends, which are simply added to start.
    ///     E.g. <c>50..^10 == 50..60</c>.
    /// </remarks>
    public static void AreInRange(Assembly assembly, Range range)
    {
        if (range.Start.IsFromEnd)
            throw new ArgumentOutOfRangeException(nameof(range), $"{nameof(Range.Start)} does not support {nameof(Index.IsFromEnd)}.");

        if (range.End.IsFromEnd)
            throw new ArgumentOutOfRangeException(nameof(range), $"{nameof(Range.End)} does not support {nameof(Index.IsFromEnd)}.");

        var min = range.Start.Value;
        var max = range.End.Value;

        if (min > max)
            throw new ArgumentOutOfRangeException(nameof(range), $"{nameof(Range.Start)} cannot be greater than {nameof(Range.End)}");

        var staticTypes = assembly.GetTypes().Where(IsStatic);
        foreach (var staticType in staticTypes)
        {
            var staticMethods = staticType.GetMethods(MethodsBindingFlags);
            foreach (var staticMethod in staticMethods)
            {
                // [LoggerMessageAttribute] has AllowMultiple = false
                var loggerMessageAttribute = staticMethod.GetCustomAttribute<LoggerMessageAttribute>();
                if (loggerMessageAttribute is null)
                    continue;

                if (loggerMessageAttribute.EventId < min || loggerMessageAttribute.EventId > max)
                    Assert.Fail($"Log {nameof(LoggerMessageAttribute.EventId)} {loggerMessageAttribute.EventId} for \"{staticType.Name}.{staticMethod.Name}\" is out of range [{min}, {max}].");
            }
        }
    }

    /// <summary>
    ///     Checks that any methods in <paramref name="assembly"/> marked with <see cref="LoggerMessageAttribute"/> have a unique <see cref="LoggerMessageAttribute.EventId"/> (within that assembly).
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to check.</param>
    public static void AreUnique(Assembly assembly)
    {
        var duplicatedEventIds = assembly
            .GetTypes()
            .Where(IsStatic)
            .SelectMany(type => type.GetMethods(MethodsBindingFlags))
            .Select(method => new
            {
                // We know this method is defined in a type as we're iterating over types to get the methods
                Type = method.DeclaringType!,
                Method = method,
                LoggerMessage = method.GetCustomAttribute<LoggerMessageAttribute>()
            })
            .Where(methodMeta => methodMeta.LoggerMessage is not null)
            .GroupBy(methodMeta => methodMeta.LoggerMessage!.EventId)
            .Where(group => group.Count() > 1)
            .ToList();

        if (duplicatedEventIds.Count == 0)
            return;

        var failMessageBuilder = new StringBuilder($"The following {nameof(EventId)}(s) were duplicated in assembly {assembly.GetName().Name ?? assembly.FullName}:\n");
        foreach (var duplicatedEventIdGroup in duplicatedEventIds)
        {
            var eventId = duplicatedEventIdGroup.Key;
            var methodIdentifiers = duplicatedEventIdGroup.Select(methodMeta => $"{methodMeta.Type.Name}.{methodMeta.Method.Name}");
            var methodIdentifiersStr = string.Join(", ", methodIdentifiers);
            failMessageBuilder.AppendLine(CultureInfo.InvariantCulture, $"   - {eventId}: {methodIdentifiersStr}");
        }

        Assert.Fail(failMessageBuilder.ToString());
    }

    private static bool IsStatic(Type type) =>
        // C# classes can't be abstract and sealed,
        // but that is how static classes are lowered to IL
        type.IsAbstract && type.IsSealed;
}
