using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Devity.Extensions
{
    public static class StringExtensions
    {
        private static readonly Regex NonAlphaNumeric = new("[^a-z0-9]+", RegexOptions.Compiled);

        /// <summary>
        /// Converts a string into a lowercase slug joined by the given separator (e.g. "Hello World!" with
        /// separator '-' becomes "hello-world"). Accented letters are transliterated to their base ASCII
        /// letter first (e.g. "Čučoriedka" becomes "cucoriedka", not dropped or split into separators), so
        /// this handles Slovak/diacritic input the way callers actually expect. Runs of remaining
        /// non-alphanumeric characters collapse to a single separator; leading/trailing separators are
        /// trimmed. Returns an empty string for blank input.
        /// </summary>
        public static string Slugify(this string str, char separator = '-')
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;

            var lowered = RemoveDiacritics(str.Trim()).ToLowerInvariant();
            var collapsed = NonAlphaNumeric.Replace(lowered, separator.ToString());
            return collapsed.Trim(separator);
        }

        /// <summary>
        /// Decomposes accented characters into a base letter plus a combining mark (Unicode NFD) and
        /// drops the combining marks, leaving the plain base letters - e.g. "á" -> "a", "č" -> "c".
        /// </summary>
        private static string RemoveDiacritics(string str)
        {
            var normalized = str.Normalize(NormalizationForm.FormD);
            var builder = new StringBuilder(normalized.Length);

            foreach (var c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
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