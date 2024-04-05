using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Tyne.Blazor;

[SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "We need to capture any exception (and not rethrow it) to avoid exceptions from virtual/abstract methods bringing the app down. The exceptions are logged.")]
public abstract class TyneFormBase<TInput, TModel> : ComponentBase, ITyneForm<TModel>, IDisposable
{
    private ILogger? Logger { get; set; }

    [Inject]
    private ILoggerFactory? LoggerFactory { get; init; }

    public FormState State { get; private set; }

    public TModel? Model { get; private set; }

    public EditForm? EditForm { get; set; }

    private readonly TaskCompletionSource _initialiseTaskSource;
    private readonly Task _onInitialised;

    protected TyneFormBase()
    {
        _initialiseTaskSource = new();
        _onInitialised = _initialiseTaskSource.Task;
    }

    private List<FormUpdatedCallback> FormUpdatedCallbacks { get; } = new();
    public IDisposable Attach(FormUpdatedCallback formUpdatedCallback)
    {
        FormUpdatedCallbacks.Add(formUpdatedCallback);
        return new DisposableAction(() => FormUpdatedCallbacks.Remove(formUpdatedCallback));
    }

    private async Task UpdateStateAsync(FormState newState)
    {
        State = newState;
        foreach (var formUpdatedCallback in FormUpdatedCallbacks)
            await formUpdatedCallback.Invoke().ConfigureAwait(true);
    }

    private readonly SemaphoreSlim _initialiseSemaphore = new(1, 1);
    public Result<Unit>? InitialiseResult { get; private set; }
    /// <summary>
    ///     Called when a form is being initialised.
    /// </summary>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> which returns a <see cref="Result{T}"/>.
    ///     If <c><see cref="Result{T}.IsOk"/> is <see langword="false"/></c>, the form will show the <see cref="Result{T}.Error"/> rather than opening.
    /// </returns>
    protected virtual Task<Result<Unit>> OnInitialiseAsync() => Result.Ok(Unit.Value).ToTask();

    private async Task InitialiseAsync()
    {
        // Return early if initialisation has already began
        if (InitialiseResult is not null)
            return;

        // Ensure we only initialise once
        // This can't happen more than once at the moment as this is only called on our first render,
        // but this may change in the future to defer initialisation til the form is opened
        await _initialiseSemaphore.WaitAsync().ConfigureAwait(true);

        try
        {
            // Check if the result has been set while we were waiting
#pragma warning disable CA1508
            // CA1508: Avoid dead conditional code
            // Despite checking above, this may have been updated while we waited for the semaphore.
            if (InitialiseResult is not null)
                return;
#pragma warning restore CA1508

            Logger = LoggerFactory?.CreateLogger(GetType());

            try
            {
                InitialiseResult = await OnInitialiseAsync().ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Logger?.LogInitialiseFormError(exception);
                InitialiseResult = Result.Error<Unit>("An unknown error occurred.");
                _initialiseTaskSource.SetResult();
                return;
            }

            _initialiseTaskSource.SetResult();
        }
        finally
        {
            // Always release our semaphore before returning
            _initialiseSemaphore.Release();
        }
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await InitialiseAsync().ConfigureAwait(true);
    }

    // Used to cancel the opening (e.g. if it is closed while loading)
    private CancellationTokenSource? _openingCancellationTokenSource;

    public Result<TModel>? OpenResult { get; private set; }
    protected abstract Task<Result<TModel>> TryOpenAsync(TInput input);
    public async Task OpenAsync(TInput input)
    {
        if (State is not FormState.Closed)
            return;

        // Start the opening
        OpenResult = null;
        await UpdateStateAsync(FormState.Loading).ConfigureAwait(true);

        // Cancel any other existing openings
        if (_openingCancellationTokenSource is CancellationTokenSource openingCts)
        {
#if NET8_0_OR_GREATER
        await openingCts.CancelAsync().ConfigureAwait(true);
#else
            openingCts.Cancel();
#endif
            openingCts.Dispose();
            _openingCancellationTokenSource = null;
        }

        // Set up our opening
        _openingCancellationTokenSource = new();
        var openingCancellationToken = _openingCancellationTokenSource.Token;

        // Make sure we're initialised first
        if (InitialiseResult is null)
        {
            try
            {
                // Await the initialisation (or for this opening to be cancelled)
                await _onInitialised.WaitAsync(openingCancellationToken).ConfigureAwait(true);
            }
            catch (TaskCanceledException)
            {
                // If opening was cancelled, we assume the canceller has handled the state, so we should just return
                return;
            }
        }

        // We can't load properly if initialisation has failed
        if (InitialiseResult?.IsOk != true)
        {
            // InitialiseResult *shouldn't* be null here...
            if (InitialiseResult is not null)
                OpenResult = Result.Error<TModel>(InitialiseResult.Error);
            else
                OpenResult = Result.Error<TModel>("Error while initialising.");

            await UpdateStateAsync(FormState.Open).ConfigureAwait(true);
            return;
        }

        try
        {
            // Await the opening (or for this opening to be cancelled)
            OpenResult = await TryOpenAsync(input).WaitAsync(openingCancellationToken).ConfigureAwait(true);
            Model = OpenResult.OrDefault();
        }
        catch (TaskCanceledException)
        {
            // If opening was cancelled, then we don't care to set the result, and we assume the canceller has handled the state, so we just return
            return;
        }
        catch (Exception exception)
        {
            OpenResult = Result.Error<TModel>("An error occurred while opening.");
            Logger?.LogOpenFormError(exception);
        }

        await UpdateStateAsync(FormState.Open).ConfigureAwait(true);
    }

    public Result<Unit>? SaveResult { get; private set; }
    protected abstract Task<Result<Unit>> TrySaveAsync(TModel model);
    protected virtual Task OnSavedAsync(TModel model) => Task.CompletedTask;
    public async Task SaveAsync()
    {
        if (State is not FormState.Open)
            return;

        if (Model is null)
            return;

        if (EditForm?.EditContext is null)
            return;

        SaveResult = null;
        await UpdateStateAsync(FormState.Saving).ConfigureAwait(true);

        var isValid = EditForm.EditContext.Validate();
        if (!isValid)
        {
            await UpdateStateAsync(FormState.Open).ConfigureAwait(true);
            return;
        }

        try
        {
            // If the model is valid, then try to save
            SaveResult = await TrySaveAsync(Model).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            SaveResult = Result.Error<Unit>("An error occurred while saving.");
            Logger?.LogSaveFormError(exception);
            await UpdateStateAsync(FormState.Open).ConfigureAwait(true);
            return;
        }

        await UpdateStateAsync(FormState.Open).ConfigureAwait(true);

        if (SaveResult.IsOk)
        {
            try
            {
                await OnSavedAsync(Model).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                Logger?.LogOnSavedFormError(exception);
            }
        }
    }

    protected virtual Task<bool> OnCloseAsync(FormCloseTrigger closeTrigger) => Task.FromResult(true);
    public async Task CloseAsync(FormCloseTrigger closeTrigger)
    {
        if (State is FormState.Closed)
            return;

        var shouldClose = true;
        try
        {
            shouldClose = await OnCloseAsync(closeTrigger).ConfigureAwait(true);
        }
        catch (Exception exception)
        {
            Logger?.LogCloseFormError(exception);
        }

        if (shouldClose)
        {
            await UpdateStateAsync(FormState.Closed).ConfigureAwait(true);

            // Cancel any other existing openings
            if (_openingCancellationTokenSource is CancellationTokenSource openingCts)
            {
#if NET8_0_OR_GREATER
                await openingCts.CancelAsync().ConfigureAwait(true);
#else
                openingCts.Cancel();
#endif
                openingCts.Dispose();
                _openingCancellationTokenSource = null;
            }

            Model = default;
            OpenResult = null;
            SaveResult = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            FormUpdatedCallbacks.Clear();
            _openingCancellationTokenSource?.Dispose();
            _initialiseSemaphore.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
