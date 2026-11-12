using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Devity.Blazor;

/// <summary>
/// Renders one toast through <see cref="Toasts.ToastTemplate"/>. It is a component rather than the bare template so
/// each toast can be keyed without wrapping it in an extra element. Public only because Razor can't use internal
/// components; it's an implementation detail of <see cref="Toasts"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class ToastSlot : ComponentBase
{
    [Parameter, EditorRequired]
    public RenderFragment<Toasts.ToastContext> Template { get; set; } = null!;

    [Parameter, EditorRequired]
    public Toasts.ToastContext Value { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(0, Template(Value));
}
