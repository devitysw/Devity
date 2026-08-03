using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Devity.Blazor;

/// <summary>
/// Base class for any Devity.Blazor component that renders translatable text. Inherit this and
/// call <see cref="Localize"/> with the English default as the key.
/// </summary>
public abstract class LocalizedComponentBase : ComponentBase
{
    [Inject]
    private IServiceProvider ServiceProvider { get; set; } = default!;

    private IStringLocalizer<DevityLocalization>? _localizer;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        // Resolved via GetService (not [Inject]) so this stays zero-config: hosts that never call
        // services.AddLocalization() simply get the English fallback below instead of a DI exception.
        _localizer = ServiceProvider.GetService<IStringLocalizer<DevityLocalization>>();
    }

    /// <summary>
    /// Looks up <paramref name="key"/> in the host app's registered localizer for the current UI
    /// culture. The key IS the English default: with no matching translation for the current
    /// culture (or no <c>services.AddLocalization()</c> at all), this returns the key unchanged.
    /// </summary>
    protected string Localize(string key) => _localizer?[key].Value ?? key;
}
