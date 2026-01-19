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
        /// Creates a normalized version of the provided input. Removing accents, spaces, dots, commas for use in searching.
        /// </summary>
        public static string NormalizeForSearch(this string input) =>
            input
                .ToLower()
                .Replace("á", "a")
                .Replace("ä", "a")
                .Replace("č", "c")
                .Replace("ď", "d")
                .Replace("é", "e")
                .Replace("ě", "e")
                .Replace("í", "i")
                .Replace("ľ", "l")
                .Replace("ĺ", "l")
                .Replace("ň", "n")
                .Replace("ó", "o")
                .Replace("ô", "o")
                .Replace("ř", "r")
                .Replace("ŕ", "r")
                .Replace("š", "s")
                .Replace("ť", "t")
                .Replace("ú", "u")
                .Replace("ý", "y")
                .Replace("ž", "z")
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(",", "")
                .Replace("’", "'");
    }
}
