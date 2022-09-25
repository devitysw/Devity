namespace Devity.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Will shorten the provided string object to the maximum of the provided maximum length. If string is shorter, full string is returned.
        /// </summary>
        public static string Shorten(this string str, int maxLength)
        {
            if (str.Length < maxLength)
                return str;

            return str[..maxLength] + "...";
        }
    }
}