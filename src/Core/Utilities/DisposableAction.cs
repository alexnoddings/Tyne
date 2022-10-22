namespace Tyne;

/// <summary>
///     Executes an <see cref="Action"/> when disposed.
/// </summary>
/// <remarks>
///		As with <see cref="IDisposable"/>, this implementation is not thread safe.
/// </remarks>
public sealed class DisposableAction : IDisposable
{
	private readonly Action _action;
	private readonly bool _onlyCallOnce;
	private bool _hasBeenDisposed;

	/// <summary>
	///     Creates a new instance of <see cref="DisposableAction"/>.
	/// </summary>
	/// <param name="action">The action to execute when <see cref="Dispose"/> is called.</param>
	/// <param name="onlyCallOnce">
	///		<para>
	///		    When <see langword="false"/>, <paramref name="action"/> will be called for every invocation of <see cref="Dispose"/>.
	///		</para>
	///		<para>
	///		    When <see langword="true"/>, <paramref name="action"/> will only be called on the first invocation of <see cref="Dispose"/>.
	///		    Subsequent calls to <see cref="Dispose"/> will simply be ignored.
	///		</para>
	/// </param>
	/// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
	public DisposableAction(Action action, bool onlyCallOnce)
	{
		_action = action ?? throw new ArgumentNullException(nameof(action));
		_onlyCallOnce = onlyCallOnce;
	}

	/// <summary>
	///     Creates a new instance of <see cref="DisposableAction"/> which will only call <paramref name="action"/> once.
	/// </summary>
	/// <inheritdoc cref="DisposableAction(Action, bool)"/>
	public DisposableAction(Action action) : this(action, true)
	{
	}

	public void Dispose()
	{
		if (!_hasBeenDisposed)
		{
			_hasBeenDisposed = true;
			_action();
		}
		else if (!_onlyCallOnce)
		{
			_action();
		}
	}
}
