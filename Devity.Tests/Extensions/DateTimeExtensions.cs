namespace Devity.Tests.Extensions;

public class DateTimeExtensionsTest
{
    [Test]
    public void ToHtmlDateStringTest()
    {
        Assert.That(new DateTime(2001, 12, 20).ToHtmlDateString(), Is.EqualTo("2001-12-20"));
        Assert.That(
            new DateTime(2001, 12, 20, 1, 1, 1).ToHtmlDateString(),
            Is.EqualTo("2001-12-20")
        );
        Assert.That(new DateTime(2099, 1, 1, 1, 1, 1).ToHtmlDateString(), Is.EqualTo("2099-01-01"));
    }

    [Test]
    public void ToHtmlStringTest()
    {
        Assert.That(
            new DateTime(2001, 12, 20, 16, 30, 0).ToHtmlString(),
            Is.EqualTo("2001-12-20T16:30")
        );
        Assert.That(
            new DateTime(2001, 12, 20, 1, 1, 1).ToHtmlString(),
            Is.EqualTo("2001-12-20T01:01")
        );
        Assert.That(
            new DateTime(2099, 1, 1, 1, 1, 1).ToHtmlString(),
            Is.EqualTo("2099-01-01T01:01")
        );
    }

    [Test]
    public void ToReadableStringTest()
    {
        Assert.That(new DateTime(2001, 12, 20).ToReadableString(), Is.EqualTo("20.12.2001"));
        Assert.That(
            new DateTime(2001, 12, 20, 1, 1, 1).ToReadableString(),
            Is.EqualTo("20.12.2001")
        );
        Assert.That(new DateTime(2099, 1, 1, 1, 1, 1).ToReadableString(), Is.EqualTo("1.1.2099"));
    }

    [Test]
    public void ToReadableStringWithTimeTest()
    {
        Assert.That(
            new DateTime(2001, 12, 20, 16, 30, 0).ToReadableStringWithTime(),
            Is.EqualTo("20.12.2001 16:30")
        );
        Assert.That(
            new DateTime(2001, 12, 20, 8, 8, 1).ToReadableStringWithTime(),
            Is.EqualTo("20.12.2001 08:08")
        );
        Assert.That(
            new DateTime(2099, 1, 1, 1, 1, 1).ToReadableStringWithTime(),
            Is.EqualTo("1.1.2099 01:01")
        );
    }

    [Test]
    public void GetUntilEndOfDayTest()
    {
        Assert.That(
            new DateTime(2023, 1, 1, 23, 59, 59).GetUntilEndOfDay(),
            Is.EqualTo(TimeSpan.FromSeconds(1))
        );
        Assert.That(
            new DateTime(2023, 5, 25, 16, 0, 0).GetUntilEndOfDay(),
            Is.EqualTo(TimeSpan.FromHours(8))
        );
        Assert.That(
            new DateTime(2023, 7, 15, 22, 30, 0).GetUntilEndOfDay(),
            Is.EqualTo(TimeSpan.FromMinutes(90))
        );
    }

    [Test]
    public void GetUntilEndOfMonthTest()
    {
        Assert.That(
            new DateTime(2023, 1, 31, 0, 0, 0).GetUntilEndOfMonth(),
            Is.EqualTo(TimeSpan.FromHours(24))
        );
        Assert.That(
            new DateTime(2023, 1, 1, 0, 0, 0).GetUntilEndOfMonth(),
            Is.EqualTo(TimeSpan.FromDays(31))
        );
        Assert.That(
            new DateTime(2023, 2, 28, 22, 30, 0).GetUntilEndOfMonth(),
            Is.EqualTo(TimeSpan.FromMinutes(90))
        );
    }

    [Test]
    public void IsWithinRangeTest()
    {
        Assert.That(
            new DateTime(2023, 1, 15).IsWithinRange(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 29)
            ),
            Is.True
        );
        Assert.That(
            new DateTime(2022, 1, 15).IsWithinRange(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 29)
            ),
            Is.False
        );
    }

    [Test]
    public void TimeSpanSumTest()
    {
        List<TimeSpan> timeSpans =
        [
            TimeSpan.FromDays(1),
            TimeSpan.FromMinutes(5),
            TimeSpan.FromSeconds(20),
        ];
        Assert.That(timeSpans.Sum(), Is.EqualTo(new TimeSpan(1, 0, 5, 20)));
    }
}
