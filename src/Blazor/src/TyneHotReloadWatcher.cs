using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using MudBlazor;
using Tyne.Blazor;

[assembly: MetadataUpdateHandler(typeof(TyneHotReloadWatcher))]

namespace Tyne.Blazor;

[SuppressMessage("Major Code Smell", "S3264: Events should be invoked", Justification = "False positive, they are invoked.")]
internal static class TyneHotReloadWatcher
{
    public static event Func<Task>? OnClearCache;
    public static event Func<Task>? OnUpdateApplication;

    // Methods are invoked by the runtime when a hot reload (metadata update) occurs
    public static void ClearCache(Type[]? _) =>
        OnClearCache?.Invoke().CatchAndLog(true);

    public static void UpdateApplication(Type[]? _) =>
        OnUpdateApplication?.Invoke().CatchAndLog(true);
}
