#if Tyne_MediatorEndpoints_GetSupport
namespace Tyne.MediatorEndpoints;

/// <summary>
///		What HTTP method an <see cref="IApiRequest"/> should be sent over.
/// </summary>
public enum ApiRequestMethod
{
    /// <summary>
    ///		Indicates that the <see cref="IApiRequest"/> should be sent over HTTP GET.
    /// </summary>
    Get,
    /// <summary>
    ///		Indicates that the <see cref="IApiRequest"/> should be sent over HTTP POST.
    /// </summary>
    Post
}
#endif
