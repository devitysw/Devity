namespace Devity.Extensions;

public static class NumberExtensions
{
    private static readonly string[] Units = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB"];

    /// <summary>
    /// Function for formatting file sizes in a human readable way, using the appropriate unit (B, KB, MB, etc.) and rounding to 2 decimal places.
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHumanReadableSize(this long bytes)
    {
        if (bytes == 0) return "0 B";
        var i = (int)Math.Floor(Math.Log(bytes, 1024));
        return $"{Math.Round(bytes / Math.Pow(1024, i), 2)} {Units[i]}";
    }
}