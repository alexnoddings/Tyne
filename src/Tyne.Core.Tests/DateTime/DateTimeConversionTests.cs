namespace Tyne;

public class DateTimeConversionTests
{
    private static DateTime DefaultUtcDateTime { get; } = new DateTime(2023, 01, 12, 14, 0, 0, DateTimeKind.Utc);

    private static DateTime DefaultLocalDateTime { get; } = DateTime.SpecifyKind(DefaultUtcDateTime, DateTimeKind.Local);

    private static DateTime DefaultUnspecifiedDateTime { get; } = DateTime.SpecifyKind(DefaultUtcDateTime, DateTimeKind.Unspecified);

    // Counter-intuitively, GMT-1 is UTC+1
    private static TimeZoneInfo TimeZoneUtc1 { get; } = TimeZoneInfo.FindSystemTimeZoneById("Etc/GMT-1");

    [Test]
    public async Task ConvertFromUtc_NullTimeZone_Throws()
    {
        var utcDateTime = DefaultUtcDateTime;
        await Assert.That(() => utcDateTime.ConvertFromUtc(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ConvertFromUtc_LocalDateTime_Throws()
    {
        var utcDateTime = DefaultLocalDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = await Assert.That(() => utcDateTime.ConvertFromUtc(timeZone)).Throws<ArgumentException>();
        await Assert.That(exception!.Message).Contains("Local", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ConvertFromUtc_UtcDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var localDateTime = utcDateTime.ConvertFromUtc(timeZone);
        await Assert.That(utcDateTime.AddHours(1)).IsEqualTo(localDateTime);
    }

    [Test]
    public async Task ConvertFromUtc_UnspecifiedDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;

        var localDateTime = utcDateTime.ConvertFromUtc(timeZone);
        await Assert.That(utcDateTime.AddHours(1)).IsEqualTo(localDateTime);
    }

    [Test]
    public async Task ConvertFromUtcAsOffset_NullTimeZone_Throws()
    {
        var utcDateTime = DefaultUtcDateTime;
        await Assert.That(() => utcDateTime.ConvertFromUtcAsOffset(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ConvertFromUtcAsOffset_LocalDateTime_Throws()
    {
        var utcDateTime = DefaultLocalDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = await Assert.That(() => utcDateTime.ConvertFromUtcAsOffset(timeZone)).Throws<ArgumentException>();
        await Assert.That(exception!.Message).Contains("Local", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ConvertFromUtcAsOffset_UtcDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(utcDateTime);

        var localDateTimeOffset = utcDateTime.ConvertFromUtcAsOffset(timeZone);
        await Assert.That(utcDateTime).IsEqualTo(localDateTimeOffset.UtcDateTime);
        await Assert.That(utcDateTime.AddHours(1)).IsEqualTo(localDateTimeOffset.DateTime);
        await Assert.That(offset).IsEqualTo(localDateTimeOffset.Offset);
    }

    [Test]
    public async Task ConvertFromUtcAsOffset_UnspecifiedDateTime_ReturnsLocal()
    {
        var utcDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(utcDateTime);

        var localDateTimeOffset = utcDateTime.ConvertFromUtcAsOffset(timeZone);
        await Assert.That(utcDateTime).IsEqualTo(localDateTimeOffset.UtcDateTime);
        await Assert.That(utcDateTime.AddHours(1)).IsEqualTo(localDateTimeOffset.DateTime);
        await Assert.That(offset).IsEqualTo(localDateTimeOffset.Offset);
    }

    [Test]
    public async Task ConvertToUtc_NullTimeZone_Throws()
    {
        var localDateTime = DefaultLocalDateTime;
        await Assert.That(() => localDateTime.ConvertToUtc(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ConvertToUtc_UtcDateTime_Throws()
    {
        var localDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = await Assert.That(() => localDateTime.ConvertToUtc(timeZone)).Throws<ArgumentException>();
        await Assert.That(exception!.Message).Contains("Utc", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ConvertToUtc_UnspecifiedDateTime_ReturnsUtc()
    {
        var localDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;

        var utcDateTime = localDateTime.ConvertToUtc(timeZone);
        await Assert.That(localDateTime.AddHours(-1)).IsEqualTo(utcDateTime);
    }

    [Test]
    public async Task ConvertToUtcAsOffset_NullTimeZone_Throws()
    {
        var localDateTime = DefaultLocalDateTime;
        await Assert.That(() => localDateTime.ConvertToUtcAsOffset(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task ConvertToUtcAsOffset_UtcDateTime_Throws()
    {
        var localDateTime = DefaultUtcDateTime;
        var timeZone = TimeZoneUtc1;

        var exception = await Assert.That(() => localDateTime.ConvertToUtcAsOffset(timeZone)).Throws<ArgumentException>();
        await Assert.That(exception!.Message).Contains("Utc", StringComparison.OrdinalIgnoreCase);
    }

    [Test]
    public async Task ConvertToUtcAsOffset_UnspecifiedDateTime_ReturnsUtc()
    {
        var localDateTime = DefaultUnspecifiedDateTime;
        var timeZone = TimeZoneUtc1;
        var offset = timeZone.GetUtcOffset(localDateTime);

        var utcDateTimeOffset = localDateTime.ConvertToUtcAsOffset(timeZone);
        await Assert.That(localDateTime.AddHours(-1)).IsEqualTo(utcDateTimeOffset.UtcDateTime);
        await Assert.That(localDateTime).IsEqualTo(utcDateTimeOffset.DateTime);
        await Assert.That(offset).IsEqualTo(utcDateTimeOffset.Offset);
    }
}
