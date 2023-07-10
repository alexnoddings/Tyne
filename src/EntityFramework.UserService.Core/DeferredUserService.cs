namespace Tyne;

internal sealed class DeferredUserService : ITyneUserService
{
    private readonly Lazy<Guid?> _userIdLazy;

    public Guid? TryGetUserId() => _userIdLazy.Value;

    public DeferredUserService(IServiceProvider serviceProvider, Func<IServiceProvider, Guid?> getUserId)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _userIdLazy = new(() => getUserId(serviceProvider), LazyThreadSafetyMode.ExecutionAndPublication);
    }
}
