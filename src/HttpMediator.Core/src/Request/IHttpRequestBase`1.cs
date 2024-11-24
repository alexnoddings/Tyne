using System.Diagnostics.CodeAnalysis;

namespace Tyne.HttpMediator;

/// <summary>
///     Represents a HTTP request which produces a <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">The type of response.</typeparam>
[SuppressMessage("Major Code Smell", "S2326: Unused type parameters should be removed.", Justification = "This is used as a generic type marker.")]
public interface IHttpRequestBase<out TResponse> : IHttpRequestMetadata
{
}
