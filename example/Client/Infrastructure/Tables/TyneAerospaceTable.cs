using MediatR;
using Microsoft.AspNetCore.Components;
using Tyne.Searching;

namespace Tyne.Aerospace.Client;

[CascadingTypeParameter(nameof(TRequest))]
[CascadingTypeParameter(nameof(TResponse))]
public class TyneAerospaceTable<TRequest, TResponse>
    : TyneTableBase<TRequest, TResponse>
    where TRequest : ISearchQuery, IRequest<SearchResults<TResponse>>, new()
{
    [Inject]
    private IMediator Mediator { get; init; } = null!;

    protected override Task<SearchResults<TResponse>> LoadDataAsync(TRequest request) =>
        Mediator.Send(request);
}
