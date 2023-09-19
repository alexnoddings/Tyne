using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Localisation;

/// <summary>
///     Base <see cref="System.DateTime"/> to <see cref="System.DateTimeOffset"/> localising component.
/// </summary>
/// <remarks>
///     <para>
///         This base implementation is agnostic of how the localised <see cref="DateTimeLocal"/> is used.
///         <see cref="TyneLocalisedDateTime"/> exposes this as a parameter, or you may inherit from this to provide a custom use.
///     </para>
///     <para>
///         <see cref="DateTimeLocal"/> is <see langword="null"/> while the user's time zone info is being loaded.
///     </para>
///     <para>
///         How the user's time zone is loaded is based on the <see cref="IUserTimeZoneService"/> implementation.
///         A JavaScript implementation is available through <see cref="TyneBuilderDateTimeLocalisationExtensions.AddUserTimeZoneFromJavascript(TyneBuilder)"/>.
///     </para>
/// </remarks>
public abstract class TyneLocalisedDateTimeBase : ComponentBase
{
    /// <summary>
    ///     The UTC date time to localise.
    /// </summary>
    /// <remarks>
    ///     This may be <see cref="DateTimeKind.Utc"/> or <see cref="DateTimeKind.Unspecified"/>, but it should NOT be <see cref="DateTimeKind.Local"/>.
    /// </remarks>
    [Parameter, EditorRequired]
    public DateTime DateTimeUtc { get; set; }
    private DateTime? _previousDateTimeUtc;

    [Inject]
    private IUserTimeZoneService UserTimeZoneService { get; init; } = null!;

    /// <summary>
    ///     The localised <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This may be <see langword="null"/> while the user's time zone is being loaded.
    ///         This is only loaded after first render to ensure that any JS is ready.
    ///     </para>
    ///     <para>
    ///         Initialisation usually takes ~200ms on Blazor WASM when loading from JavaScript.
    ///         Once initialised, this will never be <see langword="null"/> again.
    ///     </para>
    /// </remarks>
    protected DateTimeOffset? DateTimeLocal { get; private set; }

    private bool _isStarted;

    protected override Task OnParametersSetAsync()
    {
        // Ensure we aren't pre-rendering
        if (!_isStarted)
            return Task.CompletedTask;

        if (DateTimeUtc == _previousDateTimeUtc)
            return Task.CompletedTask;

        return UpdateLocalDateTimeAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _isStarted = true;
        await UpdateLocalDateTimeAsync().ConfigureAwait(true);
    }

    private async Task UpdateLocalDateTimeAsync()
    {
        var timeZone = await UserTimeZoneService.GetUserTimeZoneInfoAsync().ConfigureAwait(true);
        DateTimeLocal = DateTimeUtc.ConvertFromUtcAsOffset(timeZone);
        _previousDateTimeUtc = DateTimeUtc;
        await OnDateTimeLocalUpdatedAsync().ConfigureAwait(true);
        StateHasChanged();
    }

    /// <summary>
    ///     Called when <see cref="DateTimeLocal"/> is updated.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <remarks>
    ///     This is useful for inheritors who cache a value derived from <see cref="DateTimeLocal"/>, e.g. a humanized time.
    /// </remarks>
    protected virtual Task OnDateTimeLocalUpdatedAsync() =>
        Task.CompletedTask;
}
