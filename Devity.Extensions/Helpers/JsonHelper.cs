using System.Text.Json;

namespace Devity.Extensions.Helpers;

public static class JsonHelper
{
    /// <summary>
    /// Default options with indentation enabled
    /// </summary>
    public static readonly JsonSerializerOptions INDENTED_OPTIONS = new JsonSerializerOptions
    {
        WriteIndented = true
    };
}
