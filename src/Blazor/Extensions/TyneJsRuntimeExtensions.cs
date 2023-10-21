using Microsoft.JSInterop;

namespace Tyne.Blazor;

/// <summary>
///     Tyne extensions for <see cref="IJSRuntime"/>.
/// </summary>
/// <remarks>
///     Using Tyne's JS extensions requires Tyne's JavaScript library to be loaded by the client.
///     This can be achieved by adding the following to the page hosting Blazor:
///     <code lang="html">
///         &lt;script src="_content/Tyne.Blazor/Tyne.Blazor.js"&gt;&lt;/script&gt;
///     </code>
/// </remarks>
internal static class TyneJsRuntimeExtensions
{
    internal const string GetTimeZoneNameFunctionName = "tyneGetTimeZoneName";
    internal const string GetTimeZoneOffsetFunctionName = "tyneGetTimeZoneOffset";

    /// <summary>
    ///     Gets the user's time zone name from the <paramref name="jsRuntime"/>.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> whose result is the user's time zone name.</returns>
    /// <remarks>
    ///     <para>
    ///         Invokes the function <see cref="GetTimeZoneNameFunctionName"/>.
    ///     </para>
    ///     <para>
    ///         See the remarks on <see cref="TyneJsRuntimeExtensions"/> for using this.
    ///     </para>
    /// </remarks>
    public static ValueTask<string> GetTyneTimeZoneNameAsync(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<string>(GetTimeZoneNameFunctionName);

    /// <summary>
    ///     Gets the user's time offset from UTC in minutes from the <paramref name="jsRuntime"/>.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> whose result is the user's time zone offset from UTC in minutes.</returns>
    /// <remarks>
    ///     <para>
    ///         Invokes the function <see cref="GetTimeZoneOffsetFunctionName"/>.
    ///     </para>
    ///     <para>
    ///         See the remarks on <see cref="TyneJsRuntimeExtensions"/> for using this.
    ///     </para>
    /// </remarks>
    public static ValueTask<int> GetTyneTimeZoneOffsetAsync(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<int>(GetTimeZoneOffsetFunctionName);
}
