using Microsoft.EntityFrameworkCore;
using Tyne.Actions;
using Tyne.Docs.Data;
using Tyne.Docs.SourceGen;
using Tyne.Results;

namespace Tyne.Docs.Pages.Core.Actions;

[Sample]
public static partial class GetTopLocation
{
	public class Result
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int ItemCount { get; set; }
	}

	public class Action : BaseAction<Unit, Result>
	{
		private IDbContextFactory<DocsDbContext> DbContextFactory { get; }

		public Action(ILogger<Action> logger, IDbContextFactory<DocsDbContext> dbContextFactory) : base(logger)
		{
			DbContextFactory = dbContextFactory;
		}

		protected async override Task<Result<Result>> ExecuteAsync(Unit model)
		{
			await using var dbContext = await DbContextFactory.CreateDbContextAsync();

			return await dbContext.Locations
				.Select(location => new Result
				{
					Id = location.Id,
					Name = location.Name,
					ItemCount = location.Items.Count
				})
				.OrderByDescending(result => result.ItemCount)
				.FirstAsync();
		}
	}
}
