using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor;

public abstract class TyneFormDefaultComponent<TModel> : ComponentBase
{
    [CascadingParameter]
    protected ITyneForm<TModel> Form { get; private set; } = null!;

    protected override void OnParametersSet()
    {
        if (Form is null)
        {
            throw new InvalidOperationException(
                $"{GetType().Name} requires a cascading parameter of type {nameof(ITyneForm)}<{typeof(TModel).Name}>. "
                + "Is it being rendered inside of a TyneFormXyzContent?"
            );
        }
    }
}
