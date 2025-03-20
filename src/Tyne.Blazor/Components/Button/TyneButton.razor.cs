using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Utilities;

namespace Tyne.Blazor;

/// <summary>
///     A <see cref="MudButton"/> which will be disabled while <see cref="MudBaseButton.OnClick"/> is being executed.
///     Additionally, loading content will be displayed based on <see cref="LockVariant"/>.
/// </summary>
/// <seealso cref="MudButton"/>
public partial class TyneButton : MudButton
{
    // Whether the button is locked while running
    private bool _isLockedByExecution;

    // Whether the button is disabled by a user-supplied parameter
    private bool _disabledParameter;

    private new string Classname =>
        new CssBuilder(base.Classname)
        .AddClass("tyne-button")
        .AddClass("tyne-button-loading-bar", LockVariant is ButtonLockVariant.Bar)
        .AddClass("tyne-button-spinner", LockVariant is ButtonLockVariant.SpinnerStart or ButtonLockVariant.SpinnerEnd)
        .ToString();

    /// <summary>
    ///     The <see cref="ButtonLockVariant"/> of this button.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see cref="ButtonLockVariant.Bar"/>.
    /// </remarks>
    [Parameter]
    public ButtonLockVariant LockVariant { get; set; } = ButtonLockVariant.Bar;

    /// <summary>
    ///     Locks the button, forcing it to appear to be processing.
    /// </summary>
    [Parameter]
    public bool Locked { get; set; }

    private bool IsLocked => _isLockedByExecution || Locked;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        // Keep track of the disabled parameter state
        // This is done here rather than captured locally in OnClickHandler so that it can change while the button is executing
        if (parameters.TryGetValue(nameof(Disabled), out bool disabledParameter))
            _disabledParameter = disabledParameter;

        return base.SetParametersAsync(parameters);
    }

    protected override void OnParametersSet()
    {
        // If locked, ignore the disabled state that's been passed in
        if (IsLocked)
            Disabled = true;

        base.OnParametersSet();
    }

    protected override async Task OnClickHandler(MouseEventArgs ev)
    {
        // Ignore clicks if locked or disabled
        if (IsLocked || GetDisabledState())
            return;

        // Lock the button
        _isLockedByExecution = true;
        // Override the base disabled state, ignoring the user-specified parameter
        Disabled = true;
        // And re-render
        StateHasChanged();

        try
        {
            // Needs to continue on the captured context for the last StateHasChanged call
            await OnClick.InvokeAsync(ev).ConfigureAwait(true);
            // Activatable comes from the base implementation
            Activatable?.Activate(this, ev);
        }
        finally
        {
            // Regardless of success/failure, unlock the button
            _isLockedByExecution = false;
            // Return to the disabled state specified by the parameter
            Disabled = _disabledParameter;
            // And re-render
            StateHasChanged();
        }
    }
}
