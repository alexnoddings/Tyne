using System.Diagnostics.CodeAnalysis;

namespace Tyne;

public static partial class Option
{
    // Caches common Some Options
    internal static class Cache
    {
        public static readonly Option<Unit> SomeUnit = new(Unit.Value);
        public static readonly Option<bool> SomeTrue = new(true);
        public static readonly Option<bool> SomeFalse = new(false);
        public static readonly Option<int> SomeIntZero = new(0);
    }

    // Caches None Options
    internal static class Cache<T>
    {
        [SuppressMessage(
            "Minor Code Smell",
            "S3459: Unassigned members should be removed",
            Justification = "Needed to cache None instances."
        )]
        private static readonly Option<T> _none;

        [SuppressMessage(
            "Critical Code Smell",
            "S3218: Inner class members should not shadow outer class \"static\" or type members.",
            Justification = "This is fine for an internal class, where renaming would obscure the meaning."
        )]
        public static ref readonly Option<T> None => ref _none;
    }
}
