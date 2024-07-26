using Microsoft.AspNetCore.Components;
using Tyne.Searching;

namespace Tyne.Aerospace.Client;

[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
public class TyneAerospaceTable<TRequest, TResponse> : TyneTableBase<TRequest, TResponse> where TRequest : ISearchQuery, new()
{
    [Parameter, EditorRequired]
    public Func<TRequest, Task<SearchResults<TResponse>>>? GetData { get; set; }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (GetData is null)
            throw new InvalidOperationException($"Property '{nameof(GetData)}' must be set.");
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        SetDefault<bool>(nameof(Dense), () => Dense = true);
        SetDefault<bool>(nameof(Hover), () => Hover = true);
        SetDefault<bool>(nameof(Outlined), () => Outlined = true);
        SetDefault<bool>(nameof(Striped), () => Striped = true);
        SetDefault<int>(nameof(Elevation), () => Elevation = 0);

        return base.SetParametersAsync(parameters);

        void SetDefault<T>(string parameterName, Action setter)
        {
            // If parameter view is setting the parameter, do nothing
            if (parameters.TryGetValue(parameterName, out T? _))
                return;

            // Otherwise, run the setter for the default value
            setter();
        }
    }

    protected override Task<SearchResults<TResponse>> LoadDataAsync(TRequest request, CancellationToken cancellationToken) =>
        GetData!(request);
}
