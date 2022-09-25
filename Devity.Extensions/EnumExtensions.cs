using System.ComponentModel.DataAnnotations;

namespace Devity.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get the display name of a specific Enum value.
        /// </summary>
        public static string GetDisplayName(this Enum value)
            => value.GetType().GetMember(value.ToString()).First().GetCustomAttributes(typeof(DisplayAttribute), true)
                .Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? value.ToString();
    }
}
