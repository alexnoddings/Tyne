using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Tyne.Blazor.Persistence;

// This helps interacting with some of NavigationManagers private or less-easy-to-use methods
internal static class NavigationManagerHelper
{
    [SuppressMessage("Major Code Smell", "S3011: Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "It's better than re-implementing it.")]
    private const BindingFlags ExtensionMethodBindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

    // -------------------------------
    // GetUriWithUpdatedQueryParameter
    // -------------------------------
    private static readonly Func<string, string, string, string> GetUriWithUpdatedQueryParameterDelegate =
        MethodHelper
        .Get(
            typeof(NavigationManagerExtensions),
            "GetUriWithUpdatedQueryParameter",
            ExtensionMethodBindingFlags,
            typeof(string), typeof(string), typeof(string)
        )
        .CreateDelegate<Func<string, string, string, string>>();

    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflection to access private {nameof(NavigationManager)} methods.")]
    public static string GetUriWithUpdatedQueryParameter(string uri, string name, string value) =>
        GetUriWithUpdatedQueryParameterDelegate(uri, name, value);

    // -------------------------------
    // GetUriWithRemovedQueryParameter
    // -------------------------------
    private static readonly Func<string, string, string> GetUriWithRemovedQueryParameterDelegate =
        MethodHelper
        .Get(
            typeof(NavigationManagerExtensions),
            "GetUriWithRemovedQueryParameter",
            ExtensionMethodBindingFlags,
            typeof(string), typeof(string)
        )
        .CreateDelegate<Func<string, string, string>>();

    [RequiresUnreferencedCode($"This API is not trim safe. It relies on reflection to access private {nameof(NavigationManager)} methods.")]
    public static string GetUriWithRemovedQueryParameter(string uri, string name) =>
        GetUriWithRemovedQueryParameterDelegate(uri, name);

    // -------------------------
    // GetUriWithQueryParameters
    // -------------------------
    public static string GetUriWithQueryParameters(string uri, IReadOnlyDictionary<string, object?> parameters) =>
        // This doesn't actually *use* the navigation manager, but it does still ensure it isn't null
        NavigationManagerExtensions.GetUriWithQueryParameters(DummyNavigationManager.Instance, uri, parameters);

    private sealed class DummyNavigationManager : NavigationManager
    {
        public static DummyNavigationManager Instance { get; } = new();

        private DummyNavigationManager()
        {
        }
    }
}
