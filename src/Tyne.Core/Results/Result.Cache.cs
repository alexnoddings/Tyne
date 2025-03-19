namespace Tyne;

public static partial class Result
{
    // Caches common Ok Results.
    internal static class Cache<TE>
    {
        public static readonly Result<Unit, TE> OkUnit = new(Unit.Value);
        public static readonly Result<bool, TE> OkTrue = new(true);
        public static readonly Result<bool, TE> OkFalse = new(false);
        public static readonly Result<int, TE> OkIntZero = new(0);
        public static readonly Result<Guid, TE> OkGuidEmpty = new(Guid.Empty);
    }
}
