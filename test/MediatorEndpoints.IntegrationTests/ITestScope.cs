using Tyne.MediatorEndpoints;

namespace Tyne.MediatorEndpoints;

public interface ITestScope : IDisposable
{
    public IMediatorProxy MediatorProxy { get; }
    public IServiceProvider Services { get; }

    public void Deconstruct(out IMediatorProxy mediatorProxy, out IServiceProvider services);
}
