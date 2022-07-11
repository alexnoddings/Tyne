using Microsoft.EntityFrameworkCore;
using Tyne.Actions;
using Tyne.Docs.Data;
using Tyne.Docs.SourceGen;
using Tyne.Results;

namespace Tyne.Docs.Pages.Core.Actions;

[Sample]
public static partial class LoadLocationInfo
{
	public class Result
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int ItemCount { get; set; }

	}

	public class Action : BaseAction<Guid, Result>
	{
		private IDbContextFactory<DocsDbContext> DbContextFactory { get; }

		public Action(ILogger<Action> logger, IDbContextFactory<DocsDbContext> dbContextFactory) : base(logger)
		{
			DbContextFactory = dbContextFactory;
		}

		protected override async Task<Result<Result>> ExecuteAsync(Guid input)
		{
			await using var dbContext = await DbContextFactory.CreateDbContextAsync();

			var result = await dbContext.Locations
				.Where(location => location.Id == input)
				.Select(location => new Result
				{
					Id = location.Id,
					Name = location.Name,
					ItemCount = location.Items.Count
				})
				.FirstOrDefaultAsync();

			if (result is null)
				return Result<Result>.Failure("Location not found.");

			return result;
		}
	}
}
