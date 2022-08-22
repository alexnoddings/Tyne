﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tyne.Queries;
using Tyne.Results;

namespace Tyne.EntityFramework;

/// <summary>
///		Extends <see cref="BaseSearchAction{TQuery, TResult, TEntity}"/> to automatically inject a <see cref="DbSet{TEntity}"/>
///		into calls to <see cref="BaseSearchAction{TQuery, TResult, TEntity}.RunAsync(TQuery, IQueryable{TEntity})"/>.
/// </summary>
/// <typeparam name="TDbContext">The type of <see cref="DbContext"/> to pull <see cref="TEntity"/> from.</typeparam>
/// <inheritdoc cref="BaseSearchAction{TQuery, TResult, TEntity}"/>
public abstract class BaseSearchAction<TQuery, TResult, TEntity, TDbContext>
	: BaseSearchAction<TQuery, TResult, TEntity>
	where TQuery : ISearchQuery
	where TEntity : class
	where TDbContext : DbContext
{
	private IDbContextFactory<TDbContext> DbContextFactory { get; }

	protected BaseSearchAction(ILogger<BaseSearchAction<TQuery, TResult, TEntity, TDbContext>> logger, IDbContextFactory<TDbContext> dbContextFactory)
		: base(logger)
	{
		DbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
	}

	protected override async Task<Result<SearchResults<TResult>>> ExecuteAsync(TQuery model)
	{
		await using var dbContext = await DbContextFactory.CreateDbContextAsync();
		// We don't care about tracking for searching
		var entityQueryable = dbContext.Set<TEntity>().AsNoTracking();
		return await ExecuteAsync(model, entityQueryable);
	}
}
