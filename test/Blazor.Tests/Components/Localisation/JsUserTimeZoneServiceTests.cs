using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.JSInterop;

namespace Tyne.Blazor.Localisation;

public class JsUserTimeZoneServiceTests
{
    // Forces a ValueTask into yielding before it returns a value
    // Useful as mocked behaviour often executes fully synchronously.
    private static async ValueTask<T?> YieldReturnAsync<T>(T? value)
    {
        await Task.Yield();
        return value;
    }

    // Forces a ValueTask to delay before it returns a value
    // Useful to check concurrent calls
    private static async ValueTask<T?> DelayReturnAsync<T>(T? value, int delayMs)
    {
        await Task.Delay(delayMs);
        return value;
    }

    // Creates an IJSRuntime substitute with configurable behaviour for getTimeZoneName and getTimeZoneOffset
    [SuppressMessage("Reliability", "CA2012: Use ValueTasks correctly", Justification = "We're using them weirdly, but still correctly.")]
    private static IJSRuntime GetJsRuntimeInTimeZone(Func<ValueTask<string?>> getTimeZoneName, Func<ValueTask<int>>? getTimeZoneOffset)
    {
        var jsRuntime = Substitute.For<IJSRuntime>();

        jsRuntime
            .InvokeAsync<string?>(TyneJsRuntimeExtensions.GetTimeZoneNameFunctionName, Arg.Is<object[]>(arr => arr.Length == 0))
            .Returns(_ => getTimeZoneName());

        if (getTimeZoneOffset is not null)
        {
            jsRuntime
                .InvokeAsync<int>(TyneJsRuntimeExtensions.GetTimeZoneOffsetFunctionName, Arg.Is<object[]>(arr => arr.Length == 0))
                .Returns(_ => getTimeZoneOffset());
        }

        return jsRuntime;
    }

    private static ILogger<JsUserTimeZoneService> NullLogger { get; } = NullLogger<JsUserTimeZoneService>.Instance;

    [Fact]
    public async Task Gmt1_ReturnsCorrectTimeZone()
    {
        // Arrange
        var logger = NullLogger;
        var jsRuntime = GetJsRuntimeInTimeZone(
            getTimeZoneName: () => YieldReturnAsync("Etc/GMT+1"),
            getTimeZoneOffset: null
        );

        using var service = new JsUserTimeZoneService(logger, jsRuntime);

        // Act
        var timeZoneInfo = await service.GetUserTimeZoneInfoAsync();

        // Assert
        var expectedTimeZoneOffset = TimeSpan.FromHours(-1);
        Assert.Equal(expectedTimeZoneOffset, timeZoneInfo.BaseUtcOffset);
        Assert.False(string.IsNullOrEmpty(timeZoneInfo.DisplayName));
    }

    [Fact]
    public async Task NullTimeZone_ReturnsFallback()
    {
        const int fallbackOffsetMins = 180;

        // Arrange
        var logger = NullLogger;
        var jsRuntime = GetJsRuntimeInTimeZone(
            getTimeZoneName: () => YieldReturnAsync<string>(null),
            getTimeZoneOffset: () => YieldReturnAsync(fallbackOffsetMins)
        );

        using var service = new JsUserTimeZoneService(logger, jsRuntime);

        // Act
        var timeZoneInfo = await service.GetUserTimeZoneInfoAsync();

        // Assert
        var expectedTimeZoneOffset = TimeSpan.FromMinutes(fallbackOffsetMins);
        Assert.Equal(expectedTimeZoneOffset, timeZoneInfo.BaseUtcOffset);
        Assert.False(string.IsNullOrEmpty(timeZoneInfo.DisplayName));
    }

    [Fact]
    public async Task InvalidTimeZone_ReturnsFallback()
    {
        const int fallbackOffsetMins = 180;

        // Arrange
        var logger = NullLogger;
        var jsRuntime = GetJsRuntimeInTimeZone(
            getTimeZoneName: () => YieldReturnAsync("Tyne/UnitTestTimeZone"),
            getTimeZoneOffset: () => YieldReturnAsync(fallbackOffsetMins)
        );

        using var service = new JsUserTimeZoneService(logger, jsRuntime);

        // Act
        var timeZoneInfo = await service.GetUserTimeZoneInfoAsync();

        // Assert
        var expectedTimeZoneOffset = TimeSpan.FromMinutes(fallbackOffsetMins);
        Assert.Equal(expectedTimeZoneOffset, timeZoneInfo.BaseUtcOffset);
        Assert.False(string.IsNullOrEmpty(timeZoneInfo.DisplayName));
    }

    [Fact]
    [SuppressMessage("Reliability", "CA2012: Use ValueTasks correctly", Justification = "False positive.")]
    public async Task CachesSubsequentCalls()
    {
        // Arrange
        var logger = NullLogger;
        var jsRuntime = GetJsRuntimeInTimeZone(
            getTimeZoneName: () => YieldReturnAsync("GMT+1"),
            getTimeZoneOffset: null
        );

        using var service = new JsUserTimeZoneService(logger, jsRuntime);

        // Act
        _ = await service.GetUserTimeZoneInfoAsync();

        var timeZoneInfoTask = service.GetUserTimeZoneInfoAsync();

        // Assert
        Assert.True(timeZoneInfoTask.IsCompletedSuccessfully);

        _ = jsRuntime
            .Received(1)
            .InvokeAsync<string?>(TyneJsRuntimeExtensions.GetTimeZoneNameFunctionName, Arg.Is<object[]>(arr => arr.Length == 0));
    }

    [Fact]
    [SuppressMessage("Reliability", "CA2012: Use ValueTasks correctly", Justification = "False positive.")]
    public async Task OnlyCallsJsOnce()
    {
        // Arrange
        var logger = NullLogger;
        // Configure JS runtime to delay before returning
        var jsRuntime = GetJsRuntimeInTimeZone(
            getTimeZoneName: () => DelayReturnAsync("GMT+1", 50),
            getTimeZoneOffset: null
        );

        using var service = new JsUserTimeZoneService(logger, jsRuntime);

        // Act
        // Calls get multiple times without awaiting
        // The service should only hit the JS runtime once
        await Task.WhenAll(
            Enumerable
            .Range(0, 10)
            .Select(async _ => await service.GetUserTimeZoneInfoAsync())
        );

        // Assert
        _ = jsRuntime
            .Received(1)
            .InvokeAsync<string?>(TyneJsRuntimeExtensions.GetTimeZoneNameFunctionName, Arg.Is<object[]>(arr => arr.Length == 0));
    }
}
