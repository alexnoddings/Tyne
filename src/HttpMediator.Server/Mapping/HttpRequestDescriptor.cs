namespace Tyne.HttpMediator.Server;

/// <summary>
///     Non-generic descriptor for a HTTP mediator request type.
/// </summary>
/// <param name="RequestType">The <c>TRequest</c> <see cref="Type"/>.</param>
/// <param name="ResponseType">The <c>TResponse</c> <see cref="Type"/>.</param>
/// <param name="Metadata">Metadata about the request type.</param>
/// <remarks>
///     <paramref name="RequestType"/> should implement <see cref="IHttpRequestBase{TResponse}"/> for <paramref name="ResponseType"/>.
/// </remarks>
internal sealed record HttpRequestDescriptor(Type RequestType, Type ResponseType, ICollection<object> Metadata);
