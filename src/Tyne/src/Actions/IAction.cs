using Tyne.Results;

namespace Tyne.Actions;

/// <summary>
///     An action which is ran with a <typeparamref name="TModel"/> and produces a <see cref="Result{T}"/> of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TModel">The type of model the action takes as an input.</typeparam>
/// <typeparam name="TResult">The type of result the action produces as an output.</typeparam>
public interface IAction<TModel, TResult>
{
    /// <summary>
    ///     Runs the action.
    /// </summary>
	public Task<Result<TResult>> RunAsync(TModel model);
}
