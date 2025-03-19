using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Resources;

/// <summary>
///     Helper class for creating <see cref="ResourceManager"/>s for embedded resources.
/// </summary>
/// <remarks>
///     <para>
///         This is designed to make consuming embedded resources easier.
///     </para>
///     <para>
///         For example, <c>ExceptionMessages.cs</c> providing static access to the resources defined in <c>ErrorMessages.restext</c>:
///         <code>
///         internal class ExceptionMessages
///         {
///             private static readonly ResourceManager Resources = EmbeddedResourceManager.GetFor&lt;ExceptionMessages&gt;();
///             // Use GetString to manually specify the resource name
///             internal static readonly string UnformattedMessage = Resources.GetString(culture: null, name: "UnformattedMessage");
///             // Use GetMemberString to use the calling member's name for the resource
///             internal static string FormattedMessage(string inner) => Resources.GetMemberString(culture: null, arg0: inner);
///         }
///         </code>
///     </para>
/// </remarks>
public sealed class EmbeddedResourceManager
{
    public ResourceManager InnerResourceManager { get; }

    private static readonly CultureInfo _defaultCulture = CultureInfo.CurrentUICulture;

    private EmbeddedResourceManager(ResourceManager resourceManager)
    {
        InnerResourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
    }

    public static string GetNotFoundResourceValue(string name) => "{" + name + "}";

    private string GetStringCore(CultureInfo? culture, string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        culture ??= _defaultCulture;

        var resourceString = InnerResourceManager.GetString(name, culture);
        if (resourceString is null)
            return GetNotFoundResourceValue(name);

        return resourceString;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(CultureInfo? culture, string name) =>
        GetStringCore(culture, name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(string name) =>
        GetString(null, name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(CultureInfo? culture, string name, object? arg0) =>
        string.Format(
            culture ?? _defaultCulture,
            GetStringCore(culture, name),
            arg0
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(string name, object? arg0) =>
        GetString(null, name, arg0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(CultureInfo? culture, string name, object? arg0, object? arg1) =>
        string.Format(
            culture ?? _defaultCulture,
            GetStringCore(culture, name),
            arg0, arg1
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(string name, object? arg0, object? arg1) =>
        GetString(null, name, arg0, arg1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(CultureInfo? culture, string name, object? arg0, object? arg1, object? arg2) =>
        string.Format(
            culture ?? _defaultCulture,
            GetStringCore(culture, name),
            arg0, arg1, arg2
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(string name, object? arg0, object? arg1, object? arg2) =>
        GetString(null, name, arg0, arg1, arg2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(CultureInfo? culture, string name, object? arg0, object? arg1, object? arg2, object? arg3) =>
        string.Format(
            culture ?? _defaultCulture,
            GetStringCore(culture, name),
            arg0, arg1, arg2, arg3
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetString(string name, object? arg0, object? arg1, object? arg2, object? arg3) =>
        GetString(null, name, arg0, arg1, arg2, arg3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString(CultureInfo? culture, [CallerMemberName] string name = "") =>
        GetString(culture, name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString([CallerMemberName] string name = "") =>
        GetString(null, name);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString(CultureInfo? culture, object? arg0, [CallerMemberName] string name = "") =>
        GetString(culture, name, arg0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString(object? arg0, [CallerMemberName] string name = "") =>
        GetString(null, name, arg0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString(CultureInfo? culture, object? arg0, object? arg1, [CallerMemberName] string name = "") =>
        GetString(culture, name, arg0, arg1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string GetMemberString(object? arg0, object? arg1, [CallerMemberName] string name = "") =>
        GetString(null, name, arg0, arg1);

    public static EmbeddedResourceManager Get(string resourceName, Assembly containingAssembly)
    {
        ArgumentNullException.ThrowIfNull(resourceName);
        ArgumentNullException.ThrowIfNull(containingAssembly);

        var resourceManager = new ResourceManager($"{containingAssembly.GetName().Name}.{resourceName}", containingAssembly);
        return new(resourceManager);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EmbeddedResourceManager GetFor(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Get(type.Name, type.Assembly);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EmbeddedResourceManager GetFor<T>() =>
        GetFor(typeof(T));
}
