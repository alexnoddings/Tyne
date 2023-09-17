using MediatR;
using Microsoft.AspNetCore.Components;
using Tyne.Searching;

namespace Tyne.Blazor;

[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
// Implementation of TyneTableBase which sends TRequests through IMediator in-memory
public partial class TyneTable<TRequest, TResponse> : TyneTableBase<TRequest, TResponse>
    where TRequest : IRequest<SearchResults<TResponse>>, ISearchQuery, new()
{
    [Inject]
    private IMediator Mediator { get; init; } = null!;

    protected override async Task<SearchResults<TResponse>> LoadDataAsync(TRequest request) =>
        await Mediator
            .Send(request)
            .ConfigureAwait(true);
}
