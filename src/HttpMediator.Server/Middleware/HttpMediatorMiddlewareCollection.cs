using System.Collections.ObjectModel;

namespace Tyne.HttpMediator.Server;

/// <summary>
///     A collection of <see cref="IHttpMediatorMiddleware"/> <see cref="Type"/>s.
/// </summary>
internal sealed class HttpMediatorMiddlewareCollection : Collection<Type>
{
}
