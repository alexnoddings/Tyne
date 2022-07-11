using Microsoft.EntityFrameworkCore;
using Tyne.Actions;
using Tyne.Docs.Data;
using Tyne.Docs.SourceGen;
using Tyne.Results;

namespace Tyne.Docs.Pages.Core.Actions;

[Sample]
public static partial class CreateLocation
{
	public class Model
	{
		public string Name { get; set; } = string.Empty;
	}

	public class Action : BaseAction<Model, Unit>
	{
		private IDbContextFactory<DocsDbContext> DbContextFactory { get; }

		public Action(ILogger<Action> logger, IDbContextFactory<DocsDbContext> dbContextFactory) : base(logger)
		{
			DbContextFactory = dbContextFactory;
		}

		protected override async Task<Result<Unit>> ExecuteAsync(Model model)
		{
			await using var dbContext = await DbContextFactory.CreateDbContextAsync();

			dbContext.Locations.Add(new Location { Name = model.Name });
			await dbContext.SaveChangesAsync();

			return unit;
		}
	}
}
