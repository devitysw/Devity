using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Devity.Extensions.Helpers
{
    public static class ClassFacade
    {
        /// <summary>
        /// Gets the display name of the property or if it's not present, will use the property name instead.
        /// </summary>
        public static string GetPropertyHumanName(PropertyInfo propertyInfo)
            => propertyInfo.GetCustomAttributes(typeof(DisplayAttribute), true).Cast<DisplayAttribute>()
                .FirstOrDefault()?.Name ?? propertyInfo.Name;

        /// <summary>
        /// Returns the string representation of the object's type.
        /// </summary>
        public static string GetCleanType(object obj)
        {
            var type = obj.GetType();
            var name = type.Name;

            if (name.Contains('`'))
            {
                name = name[..name.IndexOf('`')];
                foreach (var genericType in type.GetGenericArguments())
                {
                    name += $"-{genericType.Name}";
                }

            }

            return name;
        }
    }
}
