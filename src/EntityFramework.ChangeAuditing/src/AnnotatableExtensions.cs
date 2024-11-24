using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore;

public static class AnnotatableExtensions
{
    public static bool HasAnnotation<TValue>(this IReadOnlyAnnotatable annotatable, string name, Func<TValue, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(annotatable);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(predicate);

        return annotatable.FindAnnotation(name)?.Value is TValue value && predicate(value);
    }

    public static bool HasFlagAnnotation(this IReadOnlyAnnotatable annotatable, string name) =>
        annotatable.HasAnnotation<bool>(name, b => b);
}
