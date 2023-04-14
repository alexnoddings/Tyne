using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public sealed class TyneFormFluentValidator<TModel> : ComponentBase, IDisposable where TModel : class
{
    private TyneFormFluentValidatorSubscriptions<TModel>? _subscriptions;

    private EditContext? _originalEditContext;
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = null!;

    [Parameter, EditorRequired]
    public IEnumerable<IValidator<TModel>> Validators { get; set; } = null!;

    [Parameter]
    public FormValidationEvents ValidationEvents { get; set; } = FormValidationEvents.Default;

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException(
                 $"{nameof(TyneFormFluentValidator<TModel>)} requires a cascading parameter of type {nameof(EditContext)}. " +
                 $"For example, you can use {nameof(TyneFormFluentValidator<TModel>)} inside an EditForm.");
        }

        if (Validators == null)
            throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} requires a {nameof(Validators)} parameter.");

        _originalEditContext = CurrentEditContext;
        _subscriptions = new TyneFormFluentValidatorSubscriptions<TModel>(CurrentEditContext, Validators)
        {
            ValidationEvents = ValidationEvents
        };
    }

    protected override void OnParametersSet()
    {
        if (CurrentEditContext != _originalEditContext)
            throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} does not support changing the {nameof(EditContext)} dynamically.");

        if (_subscriptions is not null)
            _subscriptions.ValidationEvents = ValidationEvents;
    }

    public void Dispose() =>
        _subscriptions?.Dispose();
}
