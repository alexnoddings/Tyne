using Tyne.HttpMediator.Client;

namespace Tyne.HttpMediator;

public interface ITestScope : IDisposable
{
    public IHttpMediator HttpMediator { get; }
    public IServiceProvider Services { get; }

    public void Deconstruct(out IHttpMediator httpMediator, out IServiceProvider services);
}
