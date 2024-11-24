namespace Tyne;

public class DateTimeConversionTests
{
    private static DateTime DefaultUtcDateTime { get; } = new DateTime(2023, 01, 12, 14, 0, 0, DateTimeKind.Utc);

    private static DateTime DefaultLocalDateTime { get; } = DateTime.SpecifyKind(DefaultUtcDateTime, DateTimeKind.Local);

    private static DateTime DefaultUnspecifiedDateTime { get; } = DateTime.SpecifyKind(DefaultUtcDateTime, DateTimeKind.Unspecified);

    // Counter-intuitively, GMT-1 is UTC+1
    private static TimeZoneInfo TimeZoneUtc1 { get; } = TimeZoneInfo.FindSystemTimeZoneById("Etc/GMT-1");

    [Fact]
    public void ConvertFromUtc_NullTimeZone_Throws()
    {
        var utcDateTime = DefaultUtcDateTime;
        _ = Assert.Throws<ArgumentNullException>(() => utcDateTime.ConvertFromUtc(null!));
    }

    [Fact]
    public void ConvertFromUtc_LocalDateTime_Throws()
    {
        var utcDateTime = DefaultLocalDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = Assert.Throws<ArgumentException>(() => utcDateTime.ConvertFromUtc(timeZone));
        Assert.Contains("Local", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertFromUtc_UtcDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var localDateTime = utcDateTime.ConvertFromUtc(timeZone);
        Assert.Equal(utcDateTime.AddHours(1), localDateTime);
    }

    [Fact]
    public void ConvertFromUtc_UnspecifiedDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;

        var localDateTime = utcDateTime.ConvertFromUtc(timeZone);
        Assert.Equal(utcDateTime.AddHours(1), localDateTime);
    }

    [Fact]
    public void ConvertFromUtcAsOffset_NullTimeZone_Throws()
    {
        var utcDateTime = DefaultUtcDateTime;
        _ = Assert.Throws<ArgumentNullException>(() => utcDateTime.ConvertFromUtcAsOffset(null!));
    }

    [Fact]
    public void ConvertFromUtcAsOffset_LocalDateTime_Throws()
    {
        var utcDateTime = DefaultLocalDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = Assert.Throws<ArgumentException>(() => utcDateTime.ConvertFromUtcAsOffset(timeZone));
        Assert.Contains("Local", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertFromUtcAsOffset_UtcDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(utcDateTime);

        var localDateTimeOffset = utcDateTime.ConvertFromUtcAsOffset(timeZone);
        Assert.Equal(utcDateTime, localDateTimeOffset.UtcDateTime);
        Assert.Equal(utcDateTime.AddHours(1), localDateTimeOffset.DateTime);
        Assert.Equal(offset, localDateTimeOffset.Offset);
    }

    [Fact]
    public void ConvertFromUtcAsOffset_UnspecifiedDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(utcDateTime);

        var localDateTimeOffset = utcDateTime.ConvertFromUtcAsOffset(timeZone);
        Assert.Equal(utcDateTime, localDateTimeOffset.UtcDateTime);
        Assert.Equal(utcDateTime.AddHours(1), localDateTimeOffset.DateTime);
        Assert.Equal(offset, localDateTimeOffset.Offset);
    }

    [Fact]
    public void ConvertToUtc_NullTimeZone_Throws()
    {
        var localDateTime = DefaultLocalDateTime;
        _ = Assert.Throws<ArgumentNullException>(() => localDateTime.ConvertToUtc(null!));
    }

    [Fact]
    public void ConvertToUtc_UtcDateTime_Throws()
    {
        var localDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = Assert.Throws<ArgumentException>(() => localDateTime.ConvertToUtc(timeZone));
        Assert.Contains("Utc", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertToUtc_UnspecifiedDateTime_ReturnsUtc()
    {
        var localDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;

        var utcDateTime = localDateTime.ConvertToUtc(timeZone);
        Assert.Equal(localDateTime.AddHours(-1), utcDateTime);
    }

    [Fact]
    public void ConvertToUtcAsOffset_NullTimeZone_Throws()
    {
        var localDateTime = DefaultLocalDateTime;
        _ = Assert.Throws<ArgumentNullException>(() => localDateTime.ConvertToUtcAsOffset(null!));
    }

    [Fact]
    public void ConvertToUtcAsOffset_UtcDateTime_Throws()
    {
        var localDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = Assert.Throws<ArgumentException>(() => localDateTime.ConvertToUtcAsOffset(timeZone));
        Assert.Contains("Utc", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConvertToUtcAsOffset_UnspecifiedDateTime_ReturnsUtc()
    {
        var localDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(localDateTime);

        var utcDateTimeOffset = localDateTime.ConvertToUtcAsOffset(timeZone);
        Assert.Equal(localDateTime.AddHours(-1), utcDateTimeOffset.UtcDateTime);
        Assert.Equal(localDateTime, utcDateTimeOffset.DateTime);
        Assert.Equal(offset, utcDateTimeOffset.Offset);
    }
}
