namespace Tyne;

public static partial class Result
{
    // Caches common Ok Results.
    internal static class Cache<E>
    {
        public static readonly Result<Unit, E> OkUnit = new(Unit.Value);
        public static readonly Result<bool, E> OkTrue = new(true);
        public static readonly Result<bool, E> OkFalse = new(false);
        public static readonly Result<int, E> OkIntZero = new(0);
        public static readonly Result<Guid, E> OkGuidEmpty = new(Guid.Empty);
    }
}
