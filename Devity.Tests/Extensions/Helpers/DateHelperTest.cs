namespace Devity.Tests.Extensions.Helpers;

public class DateHelperTest
{
    [Test]
    public void GetFirstDayOfMonthTest()
    {
        Assert.That(
            DateHelper.GetFirstDayOfMonth(new DateTime(2022, 1, 29, 15, 30, 0)),
            Is.EqualTo(new DateTime(2022, 1, 1))
        );
        Assert.That(
            DateHelper.GetFirstDayOfMonth(new DateTime(2022, 2, 13, 16, 20, 55)),
            Is.EqualTo(new DateTime(2022, 2, 1))
        );
        Assert.That(
            DateHelper.GetFirstDayOfMonth(new DateTime(2099, 3, 25)),
            Is.EqualTo(new DateTime(2099, 3, 1))
        );
        Assert.That(
            DateHelper.GetFirstDayOfMonth(),
            Is.EqualTo(DateHelper.GetFirstDayOfMonth(DateTime.Now))
        );
        Assert.That(
            DateHelper.GetFirstDayOfMonth(),
            Is.EqualTo(DateHelper.GetFirstDayOfMonth(DateTime.Today))
        );
    }

    [Test]
    public void GetLastDayOfMonthTest()
    {
        Assert.That(
            DateHelper.GetLastDayOfMonth(new DateTime(2023, 1, 29)),
            Is.EqualTo(new DateTime(2023, 1, 31))
        );
        Assert.That(
            DateHelper.GetLastDayOfMonth(new DateTime(2023, 2, 13)),
            Is.EqualTo(new DateTime(2023, 2, 28))
        );
        Assert.That(
            DateHelper.GetLastDayOfMonth(new DateTime(2023, 1, 15, 15, 30, 1)),
            Is.EqualTo(new DateTime(2023, 1, 31))
        );
        Assert.That(
            DateHelper.GetLastDayOfMonth(),
            Is.EqualTo(DateHelper.GetLastDayOfMonth(DateTime.Now))
        );
        Assert.That(
            DateHelper.GetLastDayOfMonth(),
            Is.EqualTo(DateHelper.GetLastDayOfMonth(DateTime.Today))
        );
    }

    [Test]
    public void DateAndTimeToDateTimeTest()
    {
        Assert.That(
            DateHelper.DateAndTimeToDateTime(new DateTime(2023, 1, 29), "15:15"),
            Is.EqualTo(new DateTime(2023, 1, 29, 15, 15, 0))
        );
        Assert.That(
            DateHelper.DateAndTimeToDateTime(new DateTime(2022, 1, 29), "15:15:15"),
            Is.EqualTo(new DateTime(2022, 1, 29, 15, 15, 0))
        );
        Assert.Throws<ArgumentException>(
            () => DateHelper.DateAndTimeToDateTime(new DateTime(2022, 1, 29), "eee")
        );
    }

    [Test]
    public void DoDateRangesIntersectTest()
    {
        Assert.That(
            DateHelper.DoDateRangesIntersect(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 2),
                new DateTime(2022, 1, 1),
                new DateTime(2022, 1, 2)
            ),
            Is.False
        );
        Assert.That(
            DateHelper.DoDateRangesIntersect(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 2),
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 2)
            ),
            Is.True
        );
        Assert.That(
            DateHelper.DoDateRangesIntersect(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 6),
                new DateTime(2023, 1, 2),
                new DateTime(2023, 1, 3)
            ),
            Is.True
        );
        Assert.That(
            DateHelper.DoDateRangesIntersect(
                new DateTime(2023, 1, 1),
                new DateTime(2023, 1, 2),
                new DateTime(2023, 1, 2),
                new DateTime(2023, 1, 4)
            ),
            Is.True
        );
        Assert.That(
            DateHelper.DoDateRangesIntersect(
                new DateTime(2023, 3, 15),
                new DateTime(2023, 3, 18),
                new DateTime(2023, 2, 15),
                new DateTime(2023, 3, 15)
            ),
            Is.True
        );
    }
}
