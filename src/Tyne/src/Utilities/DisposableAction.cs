namespace Tyne.Utilities;

/// <summary>
///     Executes an action when disposed.
/// </summary>
public sealed class DisposableAction : IDisposable
{
	private Action Action { get; }
	private bool OnlyCallOnce { get; }
	private bool HasBeenDisposed { get; set; }

	/// <summary>
	///     Creates a new instance of <see cref="DisposableAction"/>.
	/// </summary>
	/// <param name="action">The action to execute when <see cref="Dispose"/> is called.</param>
	/// <param name="onlyCallOnce">
	///     When <see langword="true"/>, <paramref name="action"/> will only be called on the first invocation of <see cref="Dispose"/>.
	///     When <see langword="false"/>, <paramref name="action"/> will be called for every invocation of <see cref="Dispose"/>.
	/// </param>
	/// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
	public DisposableAction(Action action, bool onlyCallOnce = true)
	{
		Action = action ?? throw new ArgumentNullException(nameof(action));
		OnlyCallOnce = onlyCallOnce;
	}

	public void Dispose()
	{
		if (!HasBeenDisposed)
		{
			HasBeenDisposed = true;
			Action();
		}
		else if (!OnlyCallOnce)
		{
			Action();
		}
	}
}
