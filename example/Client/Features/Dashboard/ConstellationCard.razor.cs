using System.Diagnostics.CodeAnalysis;

namespace Tyne.Aerospace.Client.Features.Dashboard;

public sealed partial class ConstellationCard : IDisposable
{
    private PeriodicTimer RefreshTimer { get; } = new(TimeSpan.FromMilliseconds(120));

    private double CurrentValue { get; set; } = 98.4d;
    private int MaxProgressTicks { get; } = 120;
    private int CurrentProgressTicks { get; set; } = 120;

    protected override void OnInitialized() =>
        CurrentValue = GetNextValue();

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
            return;

        _ = Task.Run(async () =>
        {
            while (await RefreshTimer.WaitForNextTickAsync())
            {
                CurrentProgressTicks--;
                if (CurrentProgressTicks == 0)
                {
                    CurrentProgressTicks = MaxProgressTicks;
                    CurrentValue = GetNextValue();
                }

                await InvokeAsync(StateHasChanged);
            }
        });
    }

    public void Dispose() =>
        RefreshTimer.Dispose();

    [SuppressMessage("Security", "CA5394: Do not use insecure randomness", Justification = "This isn't a secure operation.")]
    private double GetNextValue()
    {
        var deviation = (Random.Shared.NextDouble() - 0.5d) * 4d;
        return CurrentValue switch
        {
            >= 98d => CurrentValue - Math.Abs(deviation),
            <= 92d => CurrentValue + Math.Abs(deviation),
            _ => CurrentValue + deviation
        };
    }
}
