using System.Globalization;

namespace Tyne.Aerospace.Client.Features.Dashboard;

public sealed partial class NextLaunchCard : IDisposable
{
    private static readonly TimeSpan _launchTime = TimeSpan.Parse("19:42:00", CultureInfo.InvariantCulture);

    private PeriodicTimer Timer { get; } = new(TimeSpan.FromMilliseconds(250));

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;

        _ = Task.Run(async () =>
        {
            while (await Timer.WaitForNextTickAsync())
                await InvokeAsync(StateHasChanged);
        });
    }

    private static TimeSpan GetNextLaunchCountdown()
    {
        var now = DateTime.UtcNow;
        if (now.TimeOfDay < _launchTime)
            return _launchTime - now.TimeOfDay;
        return _launchTime + TimeSpan.FromDays(1) - now.TimeOfDay;
    }

    public void Dispose() =>
        Timer.Dispose();
}
