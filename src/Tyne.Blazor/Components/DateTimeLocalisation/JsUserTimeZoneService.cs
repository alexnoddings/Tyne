using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Tyne.Blazor.Localisation;

/// <summary>
///     An implementation of <see cref="IUserTimeZoneService"/>
///     which loads the users reported <see cref="TimeZoneInfo"/>
///     from their browser using <see cref="IJSRuntime"/>.
/// </summary>
/// <remarks>
///     This should be registered through <see cref="TyneBuilderDateTimeLocalisationExtensions.AddUserTimeZoneFromJavascript(TyneBuilder)"/>.
/// </remarks>
public sealed class JsUserTimeZoneService : IUserTimeZoneService, IDisposable
{
    private readonly ILogger<JsUserTimeZoneService> _logger;
    private readonly IJSRuntime _jsRuntime;

    private readonly SemaphoreSlim _cacheSemaphore = new(1, 1);
    private TimeZoneInfo? _cachedTimeZoneInfo;

    /// <summary>
    ///     Initialises a new <see cref="JsUserTimeZoneService"/>.
    /// </summary>
    /// <param name="logger">An <see cref="ILogger{TCategoryName}"/> instance.</param>
    /// <param name="jsRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <exception cref="ArgumentNullException">When <paramref name="logger"/> or <paramref name="jsRuntime"/> are <see langword="null"/>.</exception>
    public JsUserTimeZoneService(ILogger<JsUserTimeZoneService> logger, IJSRuntime jsRuntime)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    /// <summary>
    ///     Gets the user's local <see cref="TimeZoneInfo"/>
    /// </summary>
    /// <remarks>
    ///     This will only ever invoke the <see cref="IJSRuntime"/> once per service instance.
    ///     The <see cref="TimeZoneInfo"/> will be cached once loaded asynchronously,
    ///     and subsequent calls will be returned from the cache synchronously.
    /// </remarks>
    public ValueTask<TimeZoneInfo> GetUserTimeZoneInfoAsync()
    {
        if (_cachedTimeZoneInfo is not null)
            return ValueTask.FromResult(_cachedTimeZoneInfo);

        return GetTimeZoneInfoCoreAsync();
    }

    /// <summary>
    ///     The core logic for getting the user's <see cref="TimeZoneInfo"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Will try to load the user's reported <see cref="TimeZoneInfo"/> from the system cache first.
    ///         This is preferable as it allows for more precise localisation,
    ///         e.g. accounting for Daylight Savings Time  differences in the past/future.
    ///     </para>
    ///     <para>
    ///         If this can't be found in the system cache, a custom <see cref="TimeZoneInfo"/> will be created from
    ///         the user's reported time zone offset using <see cref="GetBackupTimeZoneInfoAsync"/>.
    ///         This is less accurate as it only works based on the user's current time zone offset.
    ///     </para>
    /// </remarks>
    private async ValueTask<TimeZoneInfo> GetTimeZoneInfoCoreAsync()
    {
        await _cacheSemaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            if (_cachedTimeZoneInfo is not null)
                return _cachedTimeZoneInfo;

            var timeZoneId = await _jsRuntime.GetTyneTimeZoneNameAsync().ConfigureAwait(false);
            var timeZone = TryFindSystemTimeZoneById(timeZoneId);
            if (timeZone is null)
            {
                _logger.LogCouldNotLoadUserTimeZone(timeZoneId);
                timeZone = await GetBackupTimeZoneInfoAsync().ConfigureAwait(false);
            }

            _cachedTimeZoneInfo = timeZone;

            return timeZone;
        }
        finally
        {
            _ = _cacheSemaphore.Release();
        }
    }

    /// <summary>
    ///     Gets a backup custom <see cref="TimeZoneInfo"/> based on the user's reported time zone offset.
    /// </summary>
    /// <remarks>
    ///     This method is not ideal as it can't account for things like Daylight Savings Time.
    ///     It is used as a backup if the user's reported time zone could not be loaded.
    /// </remarks>
    private async ValueTask<TimeZoneInfo> GetBackupTimeZoneInfoAsync()
    {
        var timeZoneOffsetMins = await _jsRuntime.GetTyneTimeZoneOffsetAsync().ConfigureAwait(false);
        var timeZoneOffset = TimeSpan.FromMinutes(timeZoneOffsetMins);

        return TimeZoneInfo.CreateCustomTimeZone("Custom/UserCurrentTimeOffset", timeZoneOffset, "Local time", null);
    }

    private static TimeZoneInfo? TryFindSystemTimeZoneById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(id);
        }
        catch (TimeZoneNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    ///     Disposes of this service's resources.
    /// </summary>
    public void Dispose() =>
        _cacheSemaphore.Dispose();
}
