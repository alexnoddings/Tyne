using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Tyne.Blazor.Forms.StateMachine;

namespace Tyne.Blazor;

[SuppressMessage(
    "Design", "CA1031: Do not catch general exception types",
    Justification = "We need to capture any exception (and not rethrow it) to avoid exceptions from virtual/abstract methods bringing the app down. The exceptions are logged."
)]
public abstract class TyneFormBase<TParams, TModel, TError> :
    ComponentBase,
    ITyneForm,
    IDisposable
{
    private FormStateMachine<TParams, TModel, TError> StateMachine { get; }

    public ITyneFormState State => StateMachine.State;

    public IDisposable WatchForStateChanges(TyneFormStateChanged stateChangedCallback) =>
        StateMachine.WatchForChanges(stateChangedCallback);

    [Parameter]
    public RenderFragment<TyneFormBase<TParams, TModel, TError>>? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<CascadingValue<ITyneForm>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<>.IsFixed), true);
        builder.AddComponentParameter(2, nameof(CascadingValue<>.Value), this);
        builder.AddComponentParameter(3, nameof(CascadingValue<>.ChildContent),
            (RenderFragment)(inner =>
            {
                inner.AddContent(0, ChildContent, this);
            })
        );
        builder.CloseComponent();
    }

    protected TyneFormBase()
    {
        StateMachine = new(TrySaveAsync);
    }

    #region Initialisation

    /// <summary>
    ///     Called when a form is being initialised.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which returns a <see cref="Result{T, E}"/>.
    ///     If the result ir an error, the form will show the error rather than any content.
    /// </returns>
    protected abstract Task<Result<TParams, TError>> InitialiseAsync();

    // Inheritors must override OnInitialisingAsync instead to avoid breaking this implementation
    protected sealed override async Task OnInitializedAsync()
    {
        // State machine should always start uninitialised
        if (StateMachine.State is TyneFormState.Supports.Initialising<TParams, TError> initialise)
        {
            await initialise.InitialiseAsync(InitialiseAsync).ConfigureAwait(false);
        }
        else
        {
            Debug.Fail($"Owned form state machine was {StateMachine.State.GetType().Name} during form initialisation.");
        }
    }
    #endregion

    protected abstract Task<Result<Unit, TError>> TrySaveAsync(TModel model);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            StateMachine.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
