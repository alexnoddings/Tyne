namespace Tyne.Aerospace.Client.Features.Systems.Filtering.DefaultValue;

public class DefaultValueExampleRequest
{
    // Do not set a default value here - it will not be used by the filter value
    public string CommanderName { get; set; } = string.Empty;
}
