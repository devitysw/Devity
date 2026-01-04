namespace Devity.Extensions
{
    public static class DateTimeExtensions
    {
        private const int SECONDS_IN_DAY = 24 * 60 * 60;
        private const int SECONDS_IN_MINUTE = 60;
        private const int SECONDS_IN_HOUR = 60 * SECONDS_IN_MINUTE;

        /// <summary>
        /// Will transform the DateTime date value to a string that can be used in HTML for setting values (also min, max, etc.).
        /// </summary>
        public static string ToHtmlDateString(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-dd");

        /// <summary>
        /// Will transform the DateTime date value to a string that can be used in HTML for setting values with the time included (also min, max, etc.).
        /// </summary>
        public static string ToHtmlString(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-ddTHH:mm");

        /// <summary>
        /// Will transform the DateTime date value to a readable string format.
        /// </summary>
        public static string ToReadableString(this DateTime dateTime) =>
            dateTime.ToString("d.M.yyyy");

        /// <summary>
        /// Will transform the DateTime date value to a readable string format with time.
        /// </summary>
        public static string ToReadableStringWithTime(this DateTime dateTime) =>
            dateTime.ToString("d.M.yyyy HH:mm");

        /// <summary>
        /// Will return the remaining TimeSpan until the end of the current day.
        /// </summary>
        /// <param name="currentTime">The object representing the current time/time we want to use to measure until end of day.</param>
        /// <returns></returns>
        public static TimeSpan GetUntilEndOfDay(this DateTime currentTime) =>
            TimeSpan.FromSeconds(
                SECONDS_IN_DAY
                    - (
                        (currentTime.Hour * SECONDS_IN_HOUR)
                        + (currentTime.Minute * SECONDS_IN_MINUTE)
                        + currentTime.Second
                    )
            );

        /// <summary>
        /// Will return the remaining TimeSpan until the end of the current month.
        /// </summary>
        /// <param name="currentTime">The object representing the current time/time we want to use to measure until end of month.</param>
        /// <returns></returns>
        public static TimeSpan GetUntilEndOfMonth(this DateTime currentTime) =>
            TimeSpan.FromSeconds(
                ((DateTime.DaysInMonth(currentTime.Year, currentTime.Month) + 1) * SECONDS_IN_DAY)
                    - (
                        (currentTime.Day * SECONDS_IN_DAY)
                        + (currentTime.Hour * SECONDS_IN_HOUR)
                        + (currentTime.Minute * SECONDS_IN_MINUTE)
                        + currentTime.Second
                    )
            );

        /// <summary>
        /// Will return whether the provided DateTime is between the two other provided DateTimes.
        /// </summary>
        public static bool IsWithinRange(
            this DateTime target,
            DateTime rangeStart,
            DateTime rangeEnd
        ) => target >= rangeStart && target <= rangeEnd;

        public static TimeSpan Sum(this IEnumerable<TimeSpan> times) =>
            times.Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
    }
}
