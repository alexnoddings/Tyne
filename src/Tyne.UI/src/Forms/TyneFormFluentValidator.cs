using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Tyne.Results;

namespace Tyne.Validation;

public sealed class TyneFormFluentValidator<TModel> : ComponentBase, IDisposable where TModel : class
{
	private TyneFormFluentValidatorSubscriptions<TModel>? _subscriptions;

	private EditContext? _originalEditContext;
	[CascadingParameter]
	private EditContext? CurrentEditContext { get; set; }

	private IValidator<TModel>? _originalValidator;
	[Parameter, EditorRequired]
	public IValidator<TModel>? Validator { get; set; }

	private Result<Unit>? _originalResult;

	[Parameter, EditorRequired]
	public Result<Unit>? Result { get; set; }

	protected override void OnInitialized()
	{
		if (CurrentEditContext == null)
			throw new InvalidOperationException(
				 $"{nameof(TyneFormFluentValidator<TModel>)} requires a cascading parameter of type {nameof(EditContext)}. " +
				 $"For example, you can use {nameof(TyneFormFluentValidator<TModel>)} inside an EditForm.");

		if (Validator == null)
			throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} requires a {nameof(Validator)} parameter of type {nameof(IValidator<TModel>)}.");

		_originalEditContext = CurrentEditContext;
		_originalValidator = Validator;
		_subscriptions = new TyneFormFluentValidatorSubscriptions<TModel>(CurrentEditContext, Validator, () => Result);
		_originalResult = Result;
	}

	protected override void OnParametersSet()
	{
		if (CurrentEditContext != _originalEditContext)
			throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} does not support changing the {nameof(EditContext)} dynamically.");

		if (Validator != _originalValidator)
			throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} does not support changing the {nameof(Validator)} dynamically.");

		if (Result != _originalResult)
		{
			_originalResult = Result;
			_subscriptions?.OnResultChanged();
		}
	}

	public void Dispose() =>
		_subscriptions?.Dispose();
}
