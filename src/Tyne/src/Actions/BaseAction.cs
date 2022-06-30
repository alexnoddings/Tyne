using Microsoft.Extensions.Logging;
using Tyne.Results;

namespace Tyne.Actions;

/// <summary>
///     The base class for an <see cref="IAction{TModel, TResult}"/>.
///     Simply handles executing the action in a try/catch block, and logs any errors which occur.
/// </summary>
/// <typeparam name="TModel">The type of model the action takes as an input.</typeparam>
/// <typeparam name="TResult">The type of result the action produces as an output.</typeparam>
public abstract class BaseAction<TModel, TResult> : IAction<TModel, TResult>
{
	protected ILogger<BaseAction<TModel, TResult>> Logger { get; }

	protected BaseAction(ILogger<BaseAction<TModel, TResult>> logger)
	{
		Logger = logger;
	}

	public virtual async Task<Result<TResult>> RunAsync(TModel model)
	{
		Result<TResult> result;
		try
		{
			result = await ExecuteAsync(model);
		}
		catch (Exception exception)
		{
			Logger.ExceptionRunningAction(exception);
			result = CommonResults.UnhandledException<TResult>(exception, "An error has occurred.");
		}

		return result;
	}

	/// <summary>
	///     Executes the action.
	/// </summary>
	/// <param name="model">The model input.</param>
	/// <remarks>This is wrapped by a try/catch block by <see cref="RunAsync(TModel)"/>, which handles logging any errors and returning a <see cref="Result{T}"/>.</remarks>
	protected abstract Task<Result<TResult>> ExecuteAsync(TModel model);
}
