namespace Tyne.HttpMediator;

/// <summary>
///     An <see cref="IHttpRequestBase{TResponse}"/> which produces a <see cref="Unit"/>.
/// </summary>
public interface IHttpRequestBase : IHttpRequestBase<Unit>
{
}
