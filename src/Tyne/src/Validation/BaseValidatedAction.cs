using FluentValidation;
using Microsoft.Extensions.Logging;
using Tyne.Actions;
using Tyne.Results;

namespace Tyne.Validation;

/// <summary>
///     A <see cref="BaseAction{TModel, TResult}"/> which is validated by <typeparamref name="TValidator"/> prior to running.
/// </summary>
/// <typeparam name="TModel">The type of model the action takes as an input.</typeparam>
/// <typeparam name="TValidator">The type of validator.</typeparam>
/// <typeparam name="TResult">The type of result the action produces as an output.</typeparam>
public abstract class BaseValidatedAction<TModel, TValidator, TResult> : BaseAction<TModel, TResult> where TValidator : AbstractValidator<TModel>
{
	protected abstract TValidator Validator { get; }

	protected BaseValidatedAction(ILogger<BaseValidatedAction<TModel, TValidator, TResult>> logger) : base(logger)
	{
	}

	public override async Task<Result<TResult>> RunAsync(TModel model)
	{
		if (!Validator.IsValid(model, out Result<TResult>? failureResult))
			return failureResult;

		return await base.RunAsync(model);
	}
}
