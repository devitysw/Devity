namespace Devity.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Will transform the DateTime date value to a string that can be used in HTML for setting values (also min, max, etc.).
        /// </summary>
        public static string DateToHtmlString(this DateTime dateTime)
            => dateTime.ToString("yyyy-MM-dd");
    }
}