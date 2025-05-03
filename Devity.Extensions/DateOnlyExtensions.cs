namespace Devity.Extensions
{
    public static class DateOnlyExtensions
    {
        /// <summary>
        /// Will transform the DateOnly date value to a readable string format.
        /// </summary>
        public static string ToReadableString(this DateOnly dateOnly) =>
            dateOnly.ToString("d.M.yyyy");
    }
}
