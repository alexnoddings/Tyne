namespace Tyne.Blazor.Localisation;

/// <summary>
///     Extensions for <see cref="IUserTimeZoneService"/> to make <see cref="DateTimeConversionExtensions"/> easier to use.
/// </summary>
public static class UserTimeZoneServiceExtensions
{
    // Converts a DateTime and TimeZoneInfo - generic to allow outputting a DateTime or DateTimeOffset
    private delegate T Converter<out T>(DateTime dateTime, TimeZoneInfo timeZone) where T : struct;

    /// <summary>
    ///     Converts <paramref name="dateTime"/> using <paramref name="userTimeZoneService"/> according to <paramref name="converter"/>.
    /// </summary>
    /// <remarks>
    ///     This aims to complete synchronously. If <see cref="IUserTimeZoneService.GetUserTimeZoneInfoAsync"/>
    ///     completes synchronously, then so will this method. Otherwise, it defers to
    ///     <see cref="ConvertCoreAsync{T}(DateTime, Converter{T}, ValueTask{TimeZoneInfo})"/>
    ///     to complete asynchronously.
    /// </remarks>
    private static ValueTask<T> ConvertCoreAsync<T>(this IUserTimeZoneService userTimeZoneService, DateTime dateTime, Converter<T> converter) where T : struct
    {
        ArgumentNullException.ThrowIfNull(userTimeZoneService);

        var timeZoneTask = userTimeZoneService.GetUserTimeZoneInfoAsync();
        if (timeZoneTask.IsCompletedSuccessfully)
        {
            // Complete synchronously if possible
            var timeZone = timeZoneTask.Result;
            var result = converter(dateTime, timeZone);

            return ValueTask.FromResult(result);
        }

        // Otherwise, complete asynchronously
        return ConvertCoreAsync(dateTime, converter, timeZoneTask);
    }

    private static async ValueTask<T> ConvertCoreAsync<T>(DateTime dateTime, Converter<T> converter, ValueTask<TimeZoneInfo> timeZoneTask) where T : struct
    {
        var timeZone = await timeZoneTask.ConfigureAwait(false);
        return converter(dateTime, timeZone);
    }

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> into a <see cref="DateTime"/> in the user's time zone.
    /// </summary>
    /// <param name="userTimeZoneService">
    ///     The <see cref="IUserTimeZoneService"/> to get the user's <see cref="TimeZoneInfo"/> from.
    /// </param>
    /// <param name="sourceDateTime">
    ///     A <see cref="DateTime"/> in UTC.
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Local"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTime"/> representing <paramref name="sourceDateTime"/> in the user's time zone.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTime"/>, which loses any offset context.
    ///     Consider using <see cref="ConvertFromUtcAsOffsetAsync(IUserTimeZoneService, DateTime)"/>
    ///     to return a <see cref="DateTimeOffset"/> which retains the offset.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="userTimeZoneService"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Local"/>.
    /// </exception>
    public static ValueTask<DateTime> ConvertFromUtcAsync(this IUserTimeZoneService userTimeZoneService, DateTime sourceDateTime) =>
        ConvertCoreAsync(userTimeZoneService, sourceDateTime, DateTimeConversionExtensions.ConvertFromUtc);

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> into a <see cref="DateTimeOffset"/> in the user's time zone.
    /// </summary>
    /// <param name="userTimeZoneService">
    ///     The <see cref="IUserTimeZoneService"/> to get the user's <see cref="TimeZoneInfo"/> from.
    /// </param>
    /// <param name="sourceDateTime">
    ///     A <see cref="DateTime"/> in UTC.
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Local"/>
    ///     as we don't know enough about the <see cref="DateTime"/> to convert it to UTC first.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTimeOffset"/> representing <paramref name="sourceDateTime"/> in user's time zone.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTimeOffset"/> which encodes the user's time zone's offset.
    ///     If this offset isn't useful, consider using <see cref="ConvertFromUtcAsync(IUserTimeZoneService, DateTime)"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="userTimeZoneService"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Local"/>.
    /// </exception>
    public static ValueTask<DateTimeOffset> ConvertFromUtcAsOffsetAsync(this IUserTimeZoneService userTimeZoneService, DateTime sourceDateTime) =>
        ConvertCoreAsync(userTimeZoneService, sourceDateTime, DateTimeConversionExtensions.ConvertFromUtcAsOffset);

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> in the user's time zone into a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="userTimeZoneService">
    ///     The <see cref="IUserTimeZoneService"/> to get the user's <see cref="TimeZoneInfo"/> from.
    /// </param>
    /// <param name="sourceDateTime">
    ///     A local <see cref="DateTime"/>.
    ///     This may be <see cref="DateTimeKind.Local"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Utc"/>.
    /// </param>
    /// <returns>
    ///     A UTC <see cref="DateTime"/> from <paramref name="sourceDateTime"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTime"/>, which loses any offset context.
    ///     Consider using <see cref="ConvertToUtcAsOffsetAsync(IUserTimeZoneService, DateTime)"/>
    ///     to return a <see cref="DateTimeOffset"/> which retains the offset.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="userTimeZoneService"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Utc"/>.
    /// </exception>
    public static ValueTask<DateTime> ConvertToUtcAsync(this IUserTimeZoneService userTimeZoneService, DateTime sourceDateTime) =>
        ConvertCoreAsync(userTimeZoneService, sourceDateTime, DateTimeConversionExtensions.ConvertToUtc);

    /// <summary>
    ///     Converts a <paramref name="sourceDateTime"/> in the user's time zone into a <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <param name="userTimeZoneService">
    ///     The <see cref="IUserTimeZoneService"/> to get the user's <see cref="TimeZoneInfo"/> from.
    /// </param>
    /// <param name="sourceDateTime">
    ///     A local <see cref="DateTime"/>.
    ///     This may be <see cref="DateTimeKind.Local"/> or <see cref="DateTimeKind.Unspecified"/>.
    ///     An <see cref="ArgumentException"/> will be thrown if it is <see cref="DateTimeKind.Utc"/>.
    /// </param>
    /// <returns>
    ///     A <see cref="DateTimeOffset"/> representing <paramref name="sourceDateTime"/> in user's time zone.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="DateTimeOffset"/> which encodes the user's time zone's offset.
    ///     If this offset isn't useful, consider using <see cref="ConvertToUtcAsync(IUserTimeZoneService, DateTime)"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     When <paramref name="userTimeZoneService"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     When <paramref name="sourceDateTime"/> is <see cref="DateTimeKind.Utc"/>.
    /// </exception>
    public static ValueTask<DateTimeOffset> ConvertToUtcAsOffsetAsync(this IUserTimeZoneService userTimeZoneService, DateTime sourceDateTime) =>
        ConvertCoreAsync(userTimeZoneService, sourceDateTime, DateTimeConversionExtensions.ConvertToUtcAsOffset);

}
