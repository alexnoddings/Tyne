namespace Tyne.Blazor;

/// <summary>
///     A filter in a <see cref="ITyneTable"/> which can configure a <typeparamref name="TRequest"/> before it is sent.
/// </summary>
/// <typeparam name="TRequest">The type of request which this filter configures.</typeparam>
public interface ITyneTableRequestFilter<in TRequest>
{
    /// <summary>
    ///     Configures the <paramref name="request"/> before it is sent.
    /// </summary>
    /// <param name="request">The <typeparamref name="TRequest"/> to configure.</param>
    /// <remarks>
    ///     This is intended to be invoked by the table, and is expected to optionally set
    ///     one or more properties on <paramref name="request"/> to filter the results returned.
    /// </remarks>
    public void ConfigureRequest(TRequest request);
}
