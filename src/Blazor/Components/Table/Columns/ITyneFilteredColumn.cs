using MudBlazor;

namespace Tyne.Blazor;

public interface ITyneFilteredColumn<in TRequest> : ITyneTableRequestFilter<TRequest>
{
    public bool IsFilterVisible { get; set; }
    public bool IsFilterActive { get; }

    public string Icon { get; }
    public Color IconColour { get; }

    public string? OrderByName { get; }

    public Task ClearValueAsync(CancellationToken cancellationToken = default);
}
