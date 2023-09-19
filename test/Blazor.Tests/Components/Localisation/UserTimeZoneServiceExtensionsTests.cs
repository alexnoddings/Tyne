using System.Diagnostics.CodeAnalysis;

namespace Tyne.Blazor.Localisation;

[SuppressMessage("Reliability", "CA2012: Use ValueTasks correctly", Justification = "We're using them weirdly, but still correctly.")]
[SuppressMessage("Critical Code Smell", "S5034: \"ValueTask\" should be consumed correctly", Justification = "As above.")]
public class UserTimeZoneServiceExtensionsTests
{
    private static DateTime DefaultUtcDateTime { get; } = new DateTime(2023, 01, 12, 14, 0, 0, DateTimeKind.Utc);

    private static DateTime DefaultUnspecifiedDateTime { get; } = DateTime.SpecifyKind(DefaultUtcDateTime, DateTimeKind.Unspecified);

    private static TimeZoneInfo TimeZoneUtc1 { get; } = TimeZoneInfo.FindSystemTimeZoneById("Etc/GMT-1");

    // GetUserTimeZoneInfoAsync is guaranteed to complete synchronously
    private static IUserTimeZoneService GetSyncUserTimeZoneService()
    {
        var userTimeZoneService = Substitute.For<IUserTimeZoneService>();

        userTimeZoneService
            .GetUserTimeZoneInfoAsync()
            .Returns(_ => ValueTask.FromResult(TimeZoneUtc1));

        return userTimeZoneService;
    }

    // GetUserTimeZoneInfoAsync is guaranteed to yield until the TCS completes (i.e. run asynchronously)
    private static IUserTimeZoneService GetAsyncUserTimeZoneService(TaskCompletionSource taskCompletionSource)
    {
        var userTimeZoneService = Substitute.For<IUserTimeZoneService>();

        async ValueTask<TimeZoneInfo> ReturnAsync()
        {
            await taskCompletionSource.Task;
            return TimeZoneUtc1;
        }

        userTimeZoneService
            .GetUserTimeZoneInfoAsync()
            .Returns(_ => ReturnAsync());

        return userTimeZoneService;
    }

    [Fact]
    public void ConvertFromUtcAsync_Sync_Works()
    {
        var service = GetSyncUserTimeZoneService();
        var task = service.ConvertFromUtcAsync(DefaultUtcDateTime);

        Assert.True(task.IsCompletedSuccessfully);
        Assert.Equal(DefaultUtcDateTime.AddHours(1), task.Result);
    }

    [Fact]
    public async Task ConvertFromUtcAsync_Async_Works()
    {
        var tcs = new TaskCompletionSource();
        var service = GetAsyncUserTimeZoneService(tcs);
        var task = service.ConvertFromUtcAsync(DefaultUtcDateTime);

        tcs.SetResult();
        var dateTime = await task;
        Assert.Equal(DefaultUtcDateTime.AddHours(1), dateTime);
    }

    [Fact]
    public void ConvertFromUtcAsOffsetAsync_Sync_Works()
    {
        var service = GetSyncUserTimeZoneService();
        var task = service.ConvertFromUtcAsOffsetAsync(DefaultUtcDateTime);

        Assert.True(task.IsCompletedSuccessfully);
        var dateTimeOffset = task.Result;
        Assert.Equal(DefaultUtcDateTime, dateTimeOffset.UtcDateTime);
        Assert.Equal(DefaultUtcDateTime.AddHours(1), dateTimeOffset.DateTime);
    }

    [Fact]
    public async Task ConvertFromUtcAsOffsetAsync_Async_Works()
    {
        var tcs = new TaskCompletionSource();
        var service = GetAsyncUserTimeZoneService(tcs);
        var task = service.ConvertFromUtcAsOffsetAsync(DefaultUtcDateTime);

        tcs.SetResult();
        var dateTimeOffset = await task;
        Assert.Equal(DefaultUtcDateTime, dateTimeOffset.UtcDateTime);
        Assert.Equal(DefaultUtcDateTime.AddHours(1), dateTimeOffset.DateTime);
    }

    [Fact]
    public void ConvertToUtcAsync_Sync_Works()
    {
        var service = GetSyncUserTimeZoneService();
        var task = service.ConvertToUtcAsync(DefaultUnspecifiedDateTime);

        Assert.True(task.IsCompletedSuccessfully);
        Assert.Equal(DefaultUnspecifiedDateTime.AddHours(-1), task.Result);
    }

    [Fact]
    public async Task ConvertToUtcAsync_Async_Works()
    {
        var tcs = new TaskCompletionSource();
        var service = GetAsyncUserTimeZoneService(tcs);
        var task = service.ConvertToUtcAsync(DefaultUnspecifiedDateTime);

        tcs.SetResult();
        var dateTime = await task;
        Assert.Equal(DefaultUnspecifiedDateTime.AddHours(-1), dateTime);
    }

    [Fact]
    public void ConvertToUtcAsOffsetAsync_Sync_Works()
    {
        var service = GetSyncUserTimeZoneService();
        var task = service.ConvertToUtcAsOffsetAsync(DefaultUnspecifiedDateTime);

        Assert.True(task.IsCompletedSuccessfully);
        var dateTimeOffset = task.Result;
        Assert.Equal(DefaultUnspecifiedDateTime.AddHours(-1), dateTimeOffset.UtcDateTime);
        Assert.Equal(DefaultUnspecifiedDateTime, dateTimeOffset.DateTime);
    }

    [Fact]
    public async Task ConvertToUtcAsOffsetAsync_Async_Works()
    {
        var tcs = new TaskCompletionSource();
        var service = GetAsyncUserTimeZoneService(tcs);
        var task = service.ConvertToUtcAsOffsetAsync(DefaultUnspecifiedDateTime);

        tcs.SetResult();
        var dateTimeOffset = await task;
        Assert.Equal(DefaultUnspecifiedDateTime.AddHours(-1), dateTimeOffset.UtcDateTime);
        Assert.Equal(DefaultUnspecifiedDateTime, dateTimeOffset.DateTime);
    }
}
