using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tyne.Blazor.Tables.Columns;

/// <summary>
///     Renders a <see cref="MudTh"/> containing <see cref="Header"/>, which has a button to open a popover with <see cref="Content"/> in.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
public abstract partial class TyneColumnPopoverHeaderBase<TResponse> : ComponentBase
{
    /// <summary>
    ///     <see langword="true"/> if the column is being actively filtered; otherwise, <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    ///     <see langword="true"/> if the <see cref="Content"/> should be shown; otherwise, <see langword="false"/>.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    ///     Invoked when the user tries to close the <see cref="Content"/>.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    ///     Which property on <typeparamref name="TResponse"/> to order by when the column is interacted with.
    /// </summary>
    /// <remarks>
    ///     Use <see cref="TyneKey.Empty"/> (or do not supply a parameter) to disable ordering on this column header.
    /// </remarks>
    [Parameter]
    public TyneKey OrderBy { get; set; }

    /// <summary>
    ///     The icon to use for the <see cref="Content"/> control.
    /// </summary>
    protected abstract string FilterIcon { get; }


    /// <summary>
    ///     The colour of the <see cref="FilterIcon"/>.
    /// </summary>
    protected abstract Color FilterIconColour { get; }

    /// <summary>
    ///     A <see cref="Func{TResult}"/> which is invoked when the 'clear value' icon is clicked.
    /// </summary>
    /// <remarks>
    ///     Leave as <see langword="null"/> to disable the 'clear value' icon button.
    /// </remarks>
    [Parameter]
    public Func<Task>? ClearValue { get; set; }

    /// <summary>
    ///     A <see cref="RenderFragment"/> to display in the column header.
    /// </summary>
    /// <remarks>
    ///     This content is always shown.
    ///     If <see langword="null"/>, <see cref="Label"/> will be used instead.
    ///     If neither property are set, the column header will be empty.
    /// </remarks>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    ///     Optionally, a label to display in the column header if <see cref="Header"/> is <see langword="null"/>.
    /// </summary>
    /// <remarks>
    ///     This content is only shown if <see cref="Header"/> is <see langword="null"/>.
    ///     Otherwise, it is ignored.
    ///     If neither property are set, the column header will be empty.
    /// </remarks>
    [Parameter]
    public string? Label { get; set; }

    private RenderFragment? GetHeaderOrLabel()
    {
        if (Header is { } header)
            return header;

        if (!string.IsNullOrEmpty(Label))
            return builder => builder.AddContent(0, Label);

        return null;
    }

    /// <summary>
    ///     The <see cref="RenderFragment"/> to display inside the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Content { get; set; }

    protected override void OnParametersSet()
    {
        // If no IsOpenChanged is passed, then the popover will
        // never open as invoking the callback won't do anything.
        // So if no delegate is provided, use a default which simply sets IsOpen.
        if (!IsOpenChanged.HasDelegate)
            IsOpenChanged = EventCallback.Factory.Create<bool>(this, isOpen => IsOpen = isOpen);
    }
}
