using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Utilities;

namespace Tyne.Blazor;

/// <summary>
///     A <see cref="MudButton"/> which will be disabled while <see cref="MudBaseButton.OnClick"/> is being executed.
///     Additionally, loading content will be displayed based on <see cref="LockStyle"/>.
/// </summary>
/// <seealso cref="MudButton"/>
public partial class TyneButton : MudButton
{
    // Whether the button is locked while running
    private bool _isLocked;

    // Whether the button is disabled by a user-supplied parameter
    private bool _disabledParameter;

    private new string Classname =>
        new CssBuilder(base.Classname)
        .AddClass("tyne-button")
        .AddClass("tyne-button-loading-bar", LockStyle is ButtonLockStyle.Bar)
        .AddClass("tyne-button-spinner", LockStyle is ButtonLockStyle.SpinnerStart or ButtonLockStyle.SpinnerEnd)
        .ToString();

    /// <summary>
    ///     The <see cref="ButtonLockStyle"/> of this button.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="ButtonLockStyle.Bar"/>.
    /// </remarks>
    [Parameter]
    public ButtonLockStyle LockStyle { get; set; } = ButtonLockStyle.Bar;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        // Keep track of the disabled parameter state
        if (parameters.TryGetValue(nameof(Disabled), out bool disabledParameter))
            _disabledParameter = disabledParameter;

        return base.SetParametersAsync(parameters);
    }

    protected override void OnParametersSet()
    {
        // If locked, ignore the disabled state that's been passed in
        if (_isLocked)
            Disabled = true;

        base.OnParametersSet();
    }

    protected override async Task OnClickHandler(MouseEventArgs ev)
    {
        // Ignore clicks if locked or disabled
        if (_isLocked || GetDisabledState())
            return;

        // Lock the button
        _isLocked = true;
        // Override the base disabled state, ignoring the user-specified parameter
        Disabled = true;
        // And re-render
        StateHasChanged();

        try
        {
            // Needs to continue on the captured context for the last StateHasChanged call
            await OnClick.InvokeAsync(ev).ConfigureAwait(true);
            // Activateable comes from the base implementation
            Activateable?.Activate(this, ev);
        }
        finally
        {
            // Regardless of success/failure, unlock the button
            _isLocked = false;
            // Return to the disabled state specified by the parameter
            Disabled = _disabledParameter;
            // And re-render
            StateHasChanged();
        }
    }
}
