using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator;

/// <summary>
///		Contains static information about an HTTP mediator request.
/// </summary>
public interface IHttpRequestMetadata
{
    /// <summary>
    ///		The relative URI to send the request to.
    /// </summary>
    [SuppressMessage("Design", "CA1056: URI-like properties should not be strings", Justification = "Strings are more ergonomic here.")]
    public static abstract string Uri { get; }

    /// <summary>
    ///     The <see cref="HttpMethod"/> to use when sending the request.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The following methods serialise the request in the URL query string:
    ///         <list type="bullet">
    ///             <item><see cref="HttpMethod.Get"/></item>
    ///             <item><see cref="HttpMethod.Delete"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         The following methods serialise the request in the content body:
    ///         <list type="bullet">
    ///             <item><see cref="HttpMethod.Post"/></item>
    ///             <item><see cref="HttpMethod.Put"/></item>
    ///             <item><see cref="HttpMethod.Patch"/></item>
    ///         </list>
    ///     </para>
    ///     <para>
    ///         The following methods are unsupported.
    ///         The server will throw an exception when trying to map them,
    ///         and the client will throw an exception when trying to send them.
    ///         <list type="bullet">
    ///             <item><see cref="HttpMethod.Connect"/></item>
    ///             <item><see cref="HttpMethod.Head"/></item>
    ///             <item><see cref="HttpMethod.Options"/></item>
    ///             <item><see cref="HttpMethod.Trace"/></item>
    ///         </list>
    ///     </para>
    /// </remarks>
    public static virtual HttpMethod Method { get; } = HttpMethod.Post;
}
