using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Tyne.Blazor;

public abstract class LocalisedDateTimeBase : ComponentBase
{
    [Parameter, EditorRequired]
    public DateTime DateTimeUtc { get; set; }
    private DateTime? _previousDateTimeUtc;

    [Inject]
    private IJSRuntime JsRuntime { get; init; } = null!;

    protected DateTimeOffset? DateTimeLocal { get; private set; }

    private bool _isStarted;

    protected override async Task OnParametersSetAsync()
    {
        // Ensure we aren't pre-rendering
        if (!_isStarted)
            return;

        if (DateTimeUtc == _previousDateTimeUtc)
            return;

        await UpdateLocalDateTimeAsync().ConfigureAwait(true);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        _isStarted = true;
        await UpdateLocalDateTimeAsync().ConfigureAwait(true);
        StateHasChanged();
    }

    private async Task UpdateLocalDateTimeAsync()
    {
        DateTimeLocal = await GetLocalDateTimeFromUtcAsync(DateTimeUtc).ConfigureAwait(true);
        _previousDateTimeUtc = DateTimeUtc;
        await OnDateTimeLocalUpdatedAsync().ConfigureAwait(true);
    }

    // Useful for subclasses which cache the DateTimeLocal, e.g. to avoid humanized times from jumping around
    protected virtual Task OnDateTimeLocalUpdatedAsync() =>
        Task.CompletedTask;

    private async ValueTask<DateTimeOffset> GetLocalDateTimeFromUtcAsync(DateTime dateTimeUtc)
    {
        if (OffsetTimeSpan is null)
            await InitialiseOffsetAsync().ConfigureAwait(true);

        if (OffsetTimeSpan is null)
            return new DateTimeOffset(DateTimeUtc, TimeSpan.Zero);

        var dateTimeOffset = dateTimeUtc + OffsetTimeSpan.Value;
        return new DateTimeOffset(dateTimeOffset, OffsetTimeSpan.Value);
    }

    private static TimeSpan? OffsetTimeSpan { get; set; }
    private static SemaphoreSlim InitialiseOffsetSemaphore { get; } = new(1, 1);
    private async Task InitialiseOffsetAsync()
    {
        await InitialiseOffsetSemaphore.WaitAsync().ConfigureAwait(true);

        try
        {
            if (OffsetTimeSpan is not null)
                return;

            var offsetMins = await JsRuntime.InvokeAsync<int>("tyneGetTimezoneOffset").ConfigureAwait(true);
            OffsetTimeSpan = TimeSpan.FromMinutes(-offsetMins);
        }
        finally
        {
            InitialiseOffsetSemaphore.Release();
        }
    }
}
