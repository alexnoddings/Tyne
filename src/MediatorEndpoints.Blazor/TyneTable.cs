using Microsoft.AspNetCore.Components;
using Tyne.Blazor.Tables;
using Tyne.MediatorEndpoints;
using Tyne.Searching;

namespace Tyne.Blazor;

[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
public partial class TyneTable<TRequest, TResponse> :
    TyneTableBase<TRequest, TResponse>
    where TRequest : IApiRequest<SearchResults<TResponse>>, ISearchQuery, new()
{
    [Inject]
    private IMediatorProxy Mediator { get; init; } = null!;

    protected override async Task<SearchResults<TResponse>> LoadDataAsync(TRequest request) =>
        await Mediator
            .Send(request)
            .ConfigureAwait(true);
}
