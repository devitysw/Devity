namespace Devity.Tests.Blazor;

public class FocusTrapTest
{
    [Test]
    public void RendersChildContent()
    {
        using var ctx = new Bunit.TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;

        var cut = ctx.RenderComponent<FocusTrap>(parameters => parameters
            .AddChildContent("Focusable content"));

        Assert.That(cut.Markup, Does.Contain("Focusable content"));
    }
}
