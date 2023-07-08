using System.Text;

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