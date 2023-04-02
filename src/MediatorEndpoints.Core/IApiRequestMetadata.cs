using System.Diagnostics.CodeAnalysis;

namespace Tyne.MediatorEndpoints;

/// <summary>
///		Contains static information about an API request.
/// </summary>
public interface IApiRequestMetadata
{
    /// <summary>
    ///		The relative URI to send the request to.
    /// </summary>
    [SuppressMessage("Design", "CA1056: URI-like properties should not be strings", Justification = "Strings are more ergonomic here.")]
    public static abstract string Uri { get; }

    #if Tyne_MediatorEndpoints_GetSupport
    /// <summary>
    ///		Which HTTP method to send the request to the API using.
    /// </summary>
    /// <seealso cref="ApiRequestMethod"/>
    public static abstract ApiRequestMethod Method { get; }
    #endif
}
