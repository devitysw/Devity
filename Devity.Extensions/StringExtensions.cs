using System.Text;
using System.Text.RegularExpressions;

namespace Devity.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex NonAlphaNumeric = new("[^a-z0-9]+", RegexOptions.Compiled);

        /// <summary>
        /// Converts a string into a lowercase slug joined by the given separator (e.g. "Hello World!" with
        /// separator '-' becomes "hello-world"). Runs of non-alphanumeric characters collapse to a single
        /// separator; leading/trailing separators are trimmed. Returns an empty string for blank input.
        /// </summary>
        public static string Slugify(this string str, char separator = '-')
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            var lowered = str.Trim().ToLowerInvariant();
            var collapsed = NonAlphaNumeric.Replace(lowered, separator.ToString());
            return collapsed.Trim(separator);
        }

        /// <summary>
        /// Will shorten the provided string object to the maximum of the provided maximum length. If string is shorter, full string is returned.
        /// </summary>
        public static string Shorten(this string str, int maxLength)
        {
            if (str.Length < maxLength)
                return str;

            return str[..maxLength] + "...";
        }

        /// <summary>
        /// Formats the provided IBAN for reading.
        /// </summary>
        public static string ToFormattedIban(this string iban)
        {
            const int groupSize = 4;
            
            iban = iban.Replace(" ", string.Empty);
            
            // Add spaces after every groupSize characters
            int totalGroups = (int)Math.Ceiling((double)iban.Length / groupSize);
            var stringBuilder = new StringBuilder();
            
            for (int i = 0; i < totalGroups; i++)
            {
                int startIndex = i * groupSize;
                int length = Math.Min(groupSize, iban.Length - startIndex);
                string group = iban.Substring(startIndex, length);
                stringBuilder.Append(group + " ");
            }
            
            return stringBuilder.ToString().Trim();
        }
    }
}