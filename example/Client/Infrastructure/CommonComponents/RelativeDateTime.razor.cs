using System.Globalization;
using Humanizer;

namespace Tyne.Aerospace.Client;

public partial class RelativeDateTime : TyneLocalisedDateTimeBase
{
    // These are cached so that recent times (e.g. < 1 min) don't jump around whenever the component renders
    private string? TimeStampString { get; set; }
    private string? HumanizedString { get; set; }

    protected override Task OnDateTimeLocalUpdatedAsync()
    {
        TimeStampString = DateTimeLocal?.ToString("yyyy'-'MM'-'dd HH':'mm':'ss zzz", CultureInfo.InvariantCulture);
        HumanizedString = DateTimeUtc.Humanize(utcDate: true, culture: CultureInfo.InvariantCulture);
        return Task.CompletedTask;
    }
}
