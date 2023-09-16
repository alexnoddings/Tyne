using MediatR;
using Microsoft.EntityFrameworkCore;
using Tyne.EntityFramework;
using Tyne.Searching;

namespace Tyne.Aerospace.Client.Infrastructure.Data;

public abstract class SearchHandler<TQuery, TResult, TEntity>
    : BaseSearchHandler<TQuery, TResult, TEntity>,
      IRequestHandler<TQuery, SearchResults<TResult>>
    where TQuery : ISearchQuery, IRequest<SearchResults<TResult>>
    where TEntity : class
{
    private readonly IAppDbContextFactory appDbContextFactory;

    protected SearchHandler(IAppDbContextFactory appDbContextFactory)
    {
        this.appDbContextFactory = appDbContextFactory ?? throw new ArgumentNullException(nameof(appDbContextFactory));
    }

    public async Task<SearchResults<TResult>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        await using var dbContext = await appDbContextFactory.CreateDbContextAsync();
        var queryable = dbContext.Set<TEntity>().AsNoTracking();
        return await ExecuteAsync(request, queryable, cancellationToken);
    }
}