namespace Devity.Extensions.Helpers;
public static class DateHelper
{
    /// <summary>
    /// Will return DateTime instance of the first day of the current month.
    /// </summary>
    public static DateTime GetFirstDayOfMonth()
        => GetFirstDayOfMonth(DateTime.Today);

    /// <summary>
    /// Will return DateTime instance of the first day of the current month in the provided baseDate instance.
    /// </summary>
    public static DateTime GetFirstDayOfMonth(DateTime baseDate)
        => new DateTime(baseDate.Year, baseDate.Month, baseDate.Day).AddDays(-(baseDate.Day - 1));

    /// <summary>
    /// Will return DateTime instance of the last day of the current month.
    /// </summary>
    public static DateTime GetLastDayOfMonth()
        => GetLastDayOfMonth(DateTime.Today);

    /// <summary>
    /// Will return DateTime instance of the last day of the current month in the provided baseDate instance.
    /// </summary>
    public static DateTime GetLastDayOfMonth(DateTime baseDate)
    {
        var firstDay = GetFirstDayOfMonth(baseDate);
        return firstDay.AddDays(DateTime.DaysInMonth(firstDay.Year, firstDay.Month)-1);
    }

    /// <summary>
    /// Will return a DateTime instance constructed from a date and a time in a string representation (must be TimeSpan parsable).
    /// </summary>
    public static DateTime DateAndTimeToDateTime(DateTime date, string time)
    {
        TimeSpan result;

        if (TimeSpan.TryParse(time, out result))
            return date.AddHours(result.Hours).AddMinutes(result.Minutes);

        throw new ArgumentException("The provided time was not parsable to a TimeSpan.");
    }
}
