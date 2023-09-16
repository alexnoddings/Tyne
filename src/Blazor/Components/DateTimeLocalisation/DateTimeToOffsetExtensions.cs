namespace Tyne.Blazor.Localisation;

/// <summary>
///     Extensions for converting <see cref="DateTime"/>s to <see cref="DateTimeOffset"/>s based in a <see cref="TimeZoneInfo"/>.
/// </summary>
public static class DateTimeToOffsetExtensions
{
    /// <summary>
    ///     Converts a <paramref name="utcDateTime"/> into a <see cref="DateTimeOffset"/> in the <paramref name="localTimeZone"/>.
    /// </summary>
    /// <param name="utcDateTime">
    ///     A <see cref="DateTime"/> in UTC.
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Local"/>
    ///     as we don't know enough about the <see cref="DateTime"/> to convert it to UTC first.
    /// </param>
    /// <param name="localTimeZone">
    ///     The <see cref="TimeZoneInfo"/> for the <see cref="DateTimeOffset"/> to be in.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTimeOffset"/> representing <paramref name="utcDateTime"/> in <paramref name="localTimeZone"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="localTimeZone"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="utcDateTime"/> is <see cref="DateTimeKind.Local"/>.
    /// </exception>
    public static DateTimeOffset ToLocalOffset(this DateTime utcDateTime, TimeZoneInfo localTimeZone)
    {
        ArgumentNullException.ThrowIfNull(localTimeZone);

        if (utcDateTime.Kind is DateTimeKind.Local)
            throw new ArgumentException($"{nameof(DateTime)} is already {nameof(DateTimeKind.Local)}.", nameof(utcDateTime));

        var destinationDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localTimeZone);
        var destinationOffset = localTimeZone.GetUtcOffset(destinationDateTime);
        return new DateTimeOffset(destinationDateTime, destinationOffset);
    }
}
