using Tyne.Results;

namespace Tyne.Actions;

/// <summary>
///     An action which is ran with a <typeparamref name="TInput"/> and produces a <see cref="Result{T}"/> of type <typeparamref name="TOutput"/>.
/// </summary>
/// <typeparam name="TInput">The type of input the action takes.</typeparam>
/// <typeparam name="TOutput">The type of output the action produces.</typeparam>
public interface IAction<TInput, TOutput>
{
	/// <summary>
	///     Runs the action.
	/// </summary>
	public Task<Result<TOutput>> RunAsync(TInput model);
}
