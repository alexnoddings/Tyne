using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Example.Client.Common.Tables;

[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
public class TyneExampleTable<TRequest, TResponse> : TyneTableBase<TRequest, TResponse> where TRequest : TyneExampleRequest, new()
{
    [Parameter, EditorRequired]
    public Func<TRequest, Task<TableData<TResponse>>>? GetData { get; set; }

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

    protected override ValueTask<TRequest> CreateRequestAsync(int page, int pageSize, string? orderBy, bool orderByDescending)
    {
        var request = new TRequest
        {
            Page = page,
            PageSize = pageSize,
            OrderBy = orderBy,
            OrderByDescending = orderByDescending
        };

        return ValueTask.FromResult(request);
    }

    protected override Task<TableData<TResponse>> LoadDataAsync(TRequest request, CancellationToken cancellationToken) =>
        GetData!(request);
}
