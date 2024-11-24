using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Tyne.Blazor;

public class TyneFormFluentValidator<TModel>
    : ComponentBase, ITyneFormFluentValidator, IDisposable
    where TModel : class
{
    private TyneFormFluentValidatorSubscriptions<TModel>? _subscriptions;
    private IDisposable? _rootValidatorRegistration;
    private bool _isDisposed;

    private EditContext? _originalEditContext;
    [CascadingParameter]
    public EditContext EditContext { get; private set; } = null!;

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

    /// <summary>
    ///     When <see langword="true"/>, this will be validated when any ancestor <see cref="TyneFormRootFluentValidator{TModel}"/>s run validation.
    ///     This is useful for forms with nested forms (e.g. an address within a customer).
    ///     However, it may not be useful for descendent forms which don't logically belong to ancestor (e.g. creating a new customer during an order creation process).
    /// </summary>
    /// <remarks>
    ///     This is only referenced during initialisation. Attempting to change it after that will throw an exception.
    ///     Defaults to <see langword="true"/>.
    /// </remarks>
    [Parameter]
    public bool UseNestedValidation { get; set; } = true;
    private bool? _originalUseNestedValidation;

    [Inject]
    private IEnumerable<IValidator<TModel>>? InjectedValidators { get; init; }

    [CascadingParameter]
    private ITyneFormRootFluentValidator? RootFluentValidator { get; set; }

    protected override void OnInitialized()
    {
        if (EditContext == null)
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

        _originalEditContext = EditContext;
        _subscriptions = new TyneFormFluentValidatorSubscriptions<TModel>(EditContext, validators)
        {
            ValidationEvents = ValidationEvents
        };

        _originalUseNestedValidation = UseNestedValidation;
        if (UseNestedValidation && RootFluentValidator is not null)
            _rootValidatorRegistration = RootFluentValidator.RegisterNestedValidator(this);
    }

    protected override void OnParametersSet()
    {
        if (UseNestedValidation != _originalUseNestedValidation)
            throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} does not support changing {nameof(UseNestedValidation)} dynamically.");

        if (EditContext != _originalEditContext)
            throw new InvalidOperationException($"{nameof(TyneFormFluentValidator<TModel>)} does not support changing the {nameof(EditContext)} dynamically.");

        if (_subscriptions is not null)
            _subscriptions.ValidationEvents = ValidationEvents;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _subscriptions?.Dispose();
                _subscriptions = null;

                _rootValidatorRegistration?.Dispose();
                _rootValidatorRegistration = null;
            }

            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
