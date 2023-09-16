using Microsoft.JSInterop;

namespace Tyne.Blazor;

internal static class TyneJsRuntimeExtensions
{
    internal const string GetTimeZoneNameFunctionName = "tyneGetTimeZoneName";
    internal const string GetTimeZoneOffsetFunctionName = "tyneGetTimeZoneOffset";

    public static ValueTask<string> GetTyneTimeZoneNameAsync(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<string>(GetTimeZoneNameFunctionName);

    public static ValueTask<int> GetTyneTimeZoneOffsetAsync(this IJSRuntime jsRuntime) =>
        jsRuntime.InvokeAsync<int>(GetTimeZoneOffsetFunctionName);
}
