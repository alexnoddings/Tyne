using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Tyne.Blazor;

public class TyneFormRootFluentValidator<TModel>
    : TyneFormFluentValidator<TModel>, ITyneFormRootFluentValidator
    where TModel : class
{
    [Parameter]
    public RenderFragment ChildContent { get; set; } = EmptyRenderFragment.Instance;

    private readonly HashSet<ITyneFormFluentValidator> _nestedValidators = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();

        EditContext.OnValidationRequested += OnValidationRequested;
    }

    public IDisposable RegisterNestedValidator(ITyneFormFluentValidator nestedValidator)
    {
        ArgumentNullException.ThrowIfNull(nestedValidator);

        var wasAdded = _nestedValidators.Add(nestedValidator);
        if (!wasAdded)
            throw new InvalidOperationException($"{nameof(nestedValidator)} has already been registered.");

        return new DisposableAction(() => _nestedValidators.Remove(nestedValidator));
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
    {
        var nestedEditContexts = _nestedValidators
            .Select(validator => validator.EditContext)
            // Guard against multiple nested validators pointing towards the same edit context
            .Distinct()
            // And against someone placing a validator inside of the root which points towards the root edit context,
            // which would cause a stack overflow as it validates itself when validation is requested
            .Where(nestedEditContext => nestedEditContext != EditContext)
            .ToList();

        foreach (var nestedEditContext in nestedEditContexts)
            _ = nestedEditContext.Validate();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<CascadingValue<ITyneFormRootFluentValidator>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<ITyneFormRootFluentValidator>.Value), this);
        builder.AddAttribute(2, nameof(CascadingValue<ITyneFormRootFluentValidator>.IsFixed), true);
        builder.AddAttribute(3, nameof(CascadingValue<ITyneFormRootFluentValidator>.ChildContent), ChildContent);
        builder.CloseComponent();
    }

    protected override void Dispose(bool disposing)
    {
        EditContext.OnValidationRequested -= OnValidationRequested;

        base.Dispose(disposing);
    }
}
