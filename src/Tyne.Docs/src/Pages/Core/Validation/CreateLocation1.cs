using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Tyne.Docs.Data;
using Tyne.Docs.SourceGen;
using Tyne.Results;
using Tyne.Validation;

namespace Tyne.Docs.Pages.Core.Validation;

[Sample]
public static partial class CreateLocation1
{
	public class Model
	{
		public string Name { get; set; } = string.Empty;
	}

	public class Validator : AbstractValidator<Model>
	{
		public static Validator Instance { get; } = new();

		public Validator()
		{
			RuleFor(model => model.Name).NotEmpty();
		}
	}

	public class Action : BaseValidatedAction<Model, Validator, Unit>
	{
		private IDbContextFactory<DocsDbContext> DbContextFactory { get; }

		protected override Validator Validator => Validator.Instance;

		public Action(ILogger<Action> logger, IDbContextFactory<DocsDbContext> dbContextFactory) : base(logger)
		{
			DbContextFactory = dbContextFactory;
		}

		protected override async Task<Result<Unit>> ExecuteAsync(Model model)
		{
			// This code will only be executed if the Validator has passed
			await using var dbContext = await DbContextFactory.CreateDbContextAsync();

			dbContext.Locations.Add(new Location { Name = model.Name });
			await dbContext.SaveChangesAsync();

			return unit;
		}
	}
}
