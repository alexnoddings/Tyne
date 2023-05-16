using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Tyne.Blazor;

public sealed class TyneFormFluentValidator<TModel> : ComponentBase, IDisposable where TModel : class
{
    private TyneFormFluentValidatorSubscriptions<TModel>? _subscriptions;

    private EditContext? _originalEditContext;
    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = null!;

    [Parameter]
    public FormValidationEvents ValidationEvents { get; set; } = FormValidationEvents.Default;

    /// <summary>
    ///     The <see cref="IValidator{T}"/>s to use for validating <typeparamref name="TModel"/>.
    /// </summary>
    /// <remarks>
    ///     These are copied during component initialisation. Changing them afterwards is not supported.
    /// </remarks>
    [Parameter]
    public IEnumerable<IValidator<TModel>>? Validators { get; set; }

    /// <summary>
    ///     <para>
    ///         When <see langword="false"/>, only the validators specified by <see cref="Validators"/> will be used for validation.
    ///     </para>
    ///     <para>
    ///         When <see langword="true"/>, <c>[Inject]</c>ed <see cref="IValidator{T}"/>s will also be used.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="true"/>.
    /// </remarks>
    [Parameter]
    public bool UseInjected { get; set; } = true;

    [Inject]
    private IEnumerable<IValidator<TModel>>? InjectedValidators { get; init; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException(
                 $"{nameof(TyneFormFluentValidator<TModel>)} requires a cascading parameter of type {nameof(EditContext)}. " +
                 $"For example, you can use {nameof(TyneFormFluentValidator<TModel>)} inside an EditForm.");
        }

        var validators = Enumerable.Empty<IValidator<TModel>>();
        if (Validators is not null)
            validators = validators.Union(Validators);
        if (UseInjected && InjectedValidators is not null)
            validators = validators.Union(InjectedValidators);

        _originalEditContext = CurrentEditContext;
        _subscriptions = new TyneFormFluentValidatorSubscriptions<TModel>(CurrentEditContext, validators)
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
