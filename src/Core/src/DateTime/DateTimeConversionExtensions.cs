using System.Runtime.CompilerServices;

namespace Tyne;

/// <summary>
///     Extensions for converting <see cref="DateTime"/>s with <see cref="TimeZoneInfo"/>s.
/// </summary>
public static class DateTimeConversionExtensions
{
    /// <summary>
    ///     Ensures <paramref name="dateTime"/> is not <paramref name="kind"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EnsureNotKind(DateTime dateTime, DateTimeKind kind, [CallerArgumentExpression(nameof(dateTime))] string? dateTimeName = null)
    {
        if (dateTime.Kind == kind)
            throw new ArgumentException($"{nameof(DateTime)} is already {kind}.", dateTimeName);
    }

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> into a <see cref="DateTime"/> in the <paramref name="targetTimeZone"/>.
    /// </summary>
    /// <param name="sourceDateTime">
    ///     A <see cref="DateTime"/> in UTC.
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Local"/>.
    /// </param>
    /// <param name="targetTimeZone">
    ///     The <see cref="TimeZoneInfo"/> for the <see cref="DateTimeOffset"/> to be in.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTime"/> representing <paramref name="sourceDateTime"/> in <paramref name="targetTimeZone"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTime"/>, which loses any offset context.
    ///     Consider using <see cref="ConvertFromUtcAsOffset(DateTime, TimeZoneInfo)"/>
    ///     to return a <see cref="DateTimeOffset"/> which retains the offset.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Local"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="targetTimeZone"/> is <see langword="null"/>.
    /// </exception>
    public static DateTime ConvertFromUtc(this DateTime sourceDateTime, TimeZoneInfo targetTimeZone)
    {
        ArgumentNullException.ThrowIfNull(targetTimeZone);
        EnsureNotKind(sourceDateTime, DateTimeKind.Local);

        return TimeZoneInfo.ConvertTimeFromUtc(sourceDateTime, targetTimeZone);
    }

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> into a <see cref="DateTimeOffset"/> in the <paramref name="targetTimeZone"/>.
    /// </summary>
    /// <param name="sourceDateTime">
    ///     A <see cref="DateTime"/> in UTC.
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Local"/>
    ///     as we don't know enough about the <see cref="DateTime"/> to convert it to UTC first.
    /// </param>
    /// <param name="targetTimeZone">
    ///     The <see cref="TimeZoneInfo"/> for the <see cref="DateTime"/> to be in.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTimeOffset"/> representing <paramref name="sourceDateTime"/> in <paramref name="targetTimeZone"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTimeOffset"/> which encodes the <paramref name="targetTimeZone"/>'s offset.
    ///     If this offset isn't useful, consider using <see cref="ConvertFromUtc(DateTime, TimeZoneInfo)"/>.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Local"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="targetTimeZone"/> is <see langword="null"/>.
    /// </exception>
    public static DateTimeOffset ConvertFromUtcAsOffset(this DateTime sourceDateTime, TimeZoneInfo targetTimeZone)
    {
        ArgumentNullException.ThrowIfNull(targetTimeZone);
        EnsureNotKind(sourceDateTime, DateTimeKind.Local);

        var targetDateTime = TimeZoneInfo.ConvertTimeFromUtc(sourceDateTime, targetTimeZone);
        var targetOffset = targetTimeZone.GetUtcOffset(targetDateTime);
        return new DateTimeOffset(targetDateTime, targetOffset);
    }

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> in the <paramref name="sourceTimeZone"/> into a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="sourceDateTime">
    ///     A local <see cref="DateTime"/>.
    ///     This may be <see cref="DateTimeKind.Local"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Utc"/>.
    /// </param>
    /// <param name="sourceTimeZone">
    ///     The <see cref="TimeZoneInfo"/> which the <paramref name="sourceDateTime"/> is in.
    /// </param>
    /// <returns>
    ///     A UTC <see cref="DateTime"/> from <paramref name="sourceDateTime"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTime"/>, which loses any offset context.
    ///     Consider using <see cref="ConvertToUtcAsOffset(DateTime, TimeZoneInfo)"/>
    ///     to return a <see cref="DateTimeOffset"/> which retains the offset.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Utc"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceTimeZone"/> is <see langword="null"/>.
    /// </exception>
    public static DateTime ConvertToUtc(this DateTime sourceDateTime, TimeZoneInfo sourceTimeZone)
    {
        ArgumentNullException.ThrowIfNull(sourceTimeZone);
        EnsureNotKind(sourceDateTime, DateTimeKind.Utc);

        return TimeZoneInfo.ConvertTimeToUtc(sourceDateTime, sourceTimeZone);
    }

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> in the <paramref name="sourceTimeZone"/> into a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="sourceDateTime">
    ///     A local <see cref="DateTime"/>.
    ///     This may be <see cref="DateTimeKind.Local"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Utc"/>.
    /// </param>
    /// <param name="sourceTimeZone">
    ///     The <see cref="TimeZoneInfo"/> which the <paramref name="sourceDateTime"/> is in.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTimeOffset"/> representing <paramref name="sourceDateTime"/> in <paramref name="sourceTimeZone"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTimeOffset"/> which encodes the <paramref name="sourceTimeZone"/>'s offset.
    ///     If this offset isn't useful, consider using <see cref="ConvertToUtc(DateTime, TimeZoneInfo)"/>.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Utc"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceTimeZone"/> is <see langword="null"/>.
    /// </exception>
    public static DateTimeOffset ConvertToUtcAsOffset(this DateTime sourceDateTime, TimeZoneInfo sourceTimeZone)
    {
        ArgumentNullException.ThrowIfNull(sourceTimeZone);
        EnsureNotKind(sourceDateTime, DateTimeKind.Utc);

        var sourceOffset = sourceTimeZone.GetUtcOffset(sourceDateTime);
        return new DateTimeOffset(sourceDateTime, sourceOffset);
    }
}
