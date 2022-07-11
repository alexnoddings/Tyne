using Microsoft.Extensions.Logging;
using Tyne.Results;

namespace Tyne.Actions;

/// <summary>
///     The base class for an <see cref="IAction{TInput, TOutput}"/>.
///     Simply handles executing the action in a try/catch block, and logs any errors which occur.
/// </summary>
/// <typeparam name="TInput">The type of model the action takes as an input.</typeparam>
/// <typeparam name="TOutput">The type of result the action produces as an output.</typeparam>
public abstract class BaseAction<TInput, TOutput> : IAction<TInput, TOutput>
{
	protected ILogger<BaseAction<TInput, TOutput>> Logger { get; }

	protected BaseAction(ILogger<BaseAction<TInput, TOutput>> logger)
	{
		Logger = logger;
	}

	public virtual async Task<Result<TOutput>> RunAsync(TInput model)
	{
		Result<TOutput> result;
		try
		{
			result = await ExecuteAsync(model);
		}
		catch (Exception exception)
		{
			Logger.ExceptionRunningAction(exception);
			result = CommonResults.UnhandledException<TOutput>(exception, "An error has occurred.");
		}

		return result;
	}

	/// <summary>
	///     Executes the action.
	/// </summary>
	/// <param name="model">The model input.</param>
	/// <remarks>This is wrapped by a try/catch block by <see cref="RunAsync(TInput)"/>, which handles logging any errors and returning a <see cref="Result{T}"/>.</remarks>
	protected abstract Task<Result<TOutput>> ExecuteAsync(TInput model);
}
