using FluentValidation;
using Microsoft.Extensions.Logging;
using Tyne.Actions;
using Tyne.Results;

namespace Tyne.Validation;

/// <summary>
///     A <see cref="BaseAction{TInput, TOutput}"/> which is validated by <typeparamref name="TValidator"/> prior to running.
/// </summary>
/// <typeparam name="TInput">The type of model the action takes as an input.</typeparam>
/// <typeparam name="TValidator">The type of validator.</typeparam>
/// <typeparam name="TOutput">The type of result the action produces as an output.</typeparam>
public abstract class BaseValidatedAction<TInput, TValidator, TOutput> : BaseAction<TInput, TOutput> where TValidator : AbstractValidator<TInput>
{
	protected abstract TValidator Validator { get; }

	protected BaseValidatedAction(ILogger<BaseValidatedAction<TInput, TValidator, TOutput>> logger) : base(logger)
	{
	}

	public override async Task<Result<TOutput>> RunAsync(TInput model)
	{
		if (!Validator.IsValid(model, out Result<TOutput>? failureResult))
			return failureResult;

		return await base.RunAsync(model);
	}
}
