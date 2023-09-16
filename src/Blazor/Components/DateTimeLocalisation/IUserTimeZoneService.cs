namespace Tyne.Blazor.Localisation;

/// <summary>
///     Fetches the user's <see cref="TimeZoneInfo"/>.
/// </summary>
public interface IUserTimeZoneService
{
    /// <summary>
    ///     Loads the current user's <see cref="TimeZoneInfo"/>.
    /// </summary>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> whose result is the current user's <see cref="TimeZoneInfo"/>.
    /// </returns>
    /// <remarks>
    ///     This returns a <see cref="ValueTask{TResult}"/> as the implementation may cache the <see cref="TimeZoneInfo"/>.
    ///     If cached, the returned <see cref="ValueTask"/> will have completed synchronously.
    /// </remarks>
    public ValueTask<TimeZoneInfo> GetUserTimeZoneInfoAsync();
}
