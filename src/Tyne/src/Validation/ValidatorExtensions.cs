using FluentValidation;
using FluentValidation.Results;
using System.Diagnostics.CodeAnalysis;
using Tyne.Results;

namespace Tyne.Validation;

/// <summary>
///     Extensions for <see cref="AbstractValidator{T}"/>.
/// </summary>
public static class ValidatorExtensions
{
    /// <summary>
    ///     Validates <paramref name="model"/> using <paramref name="validator"/>.
    /// </summary>
    /// <typeparam name="TValidator">The type of validator.</typeparam>
    /// <typeparam name="TModel">The type of model.</typeparam>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="validator">The validator to use.</param>
    /// <param name="model">The model to use.</param>
    /// <param name="failureResult">
    ///     The <see cref="Result{T}"/> of the validation.
    ///     This will be <see langword="null"/> when <see langword="true"/> is returned,
    ///     but will never be <see langword="null"/> when <see langword="false"/> is returned.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <paramref name="model"/> is valid, otherwise <see langword="false"/>.
    /// </returns>
    /// <remarks>
    ///     We return a <see langword="bool"/> and provide <paramref name="failureResult"/> as <see langword="out"/> as we can't instantiate successful <see cref="Result{T}"/>s of a generic type.
    /// </remarks>
    public static bool IsValid<TValidator, TModel, TResult>(this TValidator validator, TModel model, [NotNullWhen(false), MaybeNullWhen(true)] out Result<TResult>? failureResult)
		where TValidator : AbstractValidator<TModel>
	{
        ValidationResult validationResult = validator.Validate(model);
		if (validationResult.IsValid)
		{
            failureResult = null;
			return true;
		}

        failureResult = Result<TResult>.Failure(validationResult.Errors.Select(error => new FormValidationErrorMetadata(error.ErrorMessage, error.PropertyName)));
		return false;
	}
}
