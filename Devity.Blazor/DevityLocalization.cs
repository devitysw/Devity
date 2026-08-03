namespace Devity.Blazor;

/// <summary>
/// Marker type used purely to key the shared "DevityLocalization.*.resx" resource set (via
/// <c>IStringLocalizer&lt;DevityLocalization&gt;</c>) for every translatable string across
/// Devity.Blazor, so all components share one resx per language instead of one each.
/// </summary>
public sealed class DevityLocalization
{
    private DevityLocalization()
    {
    }
}
