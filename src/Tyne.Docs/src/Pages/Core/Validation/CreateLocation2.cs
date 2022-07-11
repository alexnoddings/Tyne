using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Tyne.Actions;
using Tyne.Docs.Data;
using Tyne.Docs.SourceGen;
using Tyne.Results;
using Tyne.Validation;

namespace Tyne.Docs.Pages.Core.Validation;

[Sample]
public static partial class CreateLocation2
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

	public class Action : BaseAction<Model, Unit>
	{
		private IDbContextFactory<DocsDbContext> DbContextFactory { get; }

		public Action(ILogger<Action> logger, IDbContextFactory<DocsDbContext> dbContextFactory) : base(logger)
		{
			DbContextFactory = dbContextFactory;
		}

		protected override async Task<Result<Unit>> ExecuteAsync(Model model)
		{
			// Note that you would usually use BaseValidatedAction to validate an action.

			// failureResult will be populated with validation error metadata if validation failed
			// If validation succeded, failureResult will be null - this is as we can't produce a
			// valid, default value for a generic T.
			if (!Validator.Instance.IsValid(model, out Result<Unit>? failureResult))
				return failureResult;

			await using var dbContext = await DbContextFactory.CreateDbContextAsync();

			dbContext.Locations.Add(new Location { Name = model.Name });
			await dbContext.SaveChangesAsync();

			return unit;
		}
	}
}
