namespace Tyne.Blazor;

/// <summary>
///     An interface for getting the current <see cref="EnvironmentName"/>.
/// </summary>
/// <remarks>
///     This is needed as there isn't a seemingly consistent way of doing this between WASM and Server.
///     WASM uses IWebAssemblyHostEnvironment (which requires the WASM), and Server uses IHostEnvironment (which isn't available in Browser).
/// </remarks>
public interface IEnvironment
{
    /// <summary>
    ///     Gets the name of the environment.
    /// </summary>
    public string EnvironmentName { get; }
}
