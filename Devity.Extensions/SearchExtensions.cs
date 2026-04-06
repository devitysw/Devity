using System.Globalization;
using System.Text;

namespace Devity.Extensions
{
    public static class SearchExtensions
    {
        /// <summary>
        /// Searches for a normalized version of the haystack in the normalized version of the needle.
        /// </summary>
        public static bool Has(this string haystack, string needle)
        {
            return haystack.Contains(needle) || haystack.NormalizeForSearch().Contains(needle.NormalizeForSearch());
        }

        /// <summary>
        /// Creates a normalized version of the provided input by removing diacritics, spaces, dots, and commas for use in searching.
        /// </summary>
        public static string NormalizeForSearch(this string input)
        {
            var normalized = input
                .ToLowerInvariant()
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("’", "'")
                .Normalize(NormalizationForm.FormD);

            var builder = new StringBuilder(normalized.Length);

            foreach (var character in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
