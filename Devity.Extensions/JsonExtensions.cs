using Devity.Extensions.Helpers;
using System.Text.Json;

namespace Devity.Extensions;

public static class JsonExtensions
{
    /// <summary>
    /// Function that returns a JSON in a quicker way, with indentation enabled by default.
    /// </summary>
    /// <param name="obj">Object to be JSONified</param>
    /// <param name="indented">Whether the resulting JSON should use indentation.</param>
    /// <returns>JSON version of the provided object.</returns>
    public static string ToJson(this object obj, bool indented = true) =>
        indented
            ? JsonSerializer.Serialize(obj, JsonHelper.INDENTED_OPTIONS)
            : JsonSerializer.Serialize(obj);
}
