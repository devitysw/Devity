namespace Devity.Tests.Blazor;

public class DevityDialogTest
{
    [Test]
    public async Task ShowRendersDialogContent()
    {
        using var ctx = new Bunit.TestContext();
        SetupDialogJavascript(ctx);

        var cut = ctx.RenderComponent<DevityDialog>(parameters => parameters
            .Add(p => p.Title, "Confirm action")
            .AddChildContent("Dialog body"));

        await cut.InvokeAsync(cut.Instance.Show);

        Assert.That(cut.Instance.IsOpen, Is.True);
        Assert.That(cut.Instance.IsShown, Is.True);
        Assert.That(cut.Markup, Does.Contain("Confirm action"));
        Assert.That(cut.Markup, Does.Contain("Dialog body"));
        Assert.That(cut.Find("[role=dialog]"), Is.Not.Null);
    }

    [Test]
    public async Task CancelInvokesCallbacksAndCloses()
    {
        using var ctx = new Bunit.TestContext();
        SetupDialogJavascript(ctx);
        var cancelled = false;
        var closed = false;

        var cut = ctx.RenderComponent<DevityDialog>(parameters => parameters
            .Add(p => p.Title, "Confirm action")
            .Add(p => p.AnimationMilliseconds, 1)
            .Add(p => p.OnCancel, () => cancelled = true)
            .Add(p => p.OnClose, () => closed = true)
            .AddChildContent("Dialog body"));

        await cut.InvokeAsync(cut.Instance.Show);
        await cut.Find("button[aria-label=Close]").ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.That(cancelled, Is.True);
        Assert.That(closed, Is.True);
        Assert.That(cut.Instance.IsOpen, Is.False);
    }

    [Test]
    public async Task SubmitInvokesSubmitAttemptedWithoutClosing()
    {
        using var ctx = new Bunit.TestContext();
        SetupDialogJavascript(ctx);
        var submitted = false;

        var cut = ctx.RenderComponent<DevityDialog>(parameters => parameters
            .Add(p => p.Title, "Confirm action")
            .Add(p => p.OnSubmitAttempted, () => submitted = true)
            .AddChildContent("Dialog body"));

        await cut.InvokeAsync(cut.Instance.Show);
        await cut.FindAll("button").Last().ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

        Assert.That(submitted, Is.True);
        Assert.That(cut.Instance.IsOpen, Is.True);
    }

    private static void SetupDialogJavascript(Bunit.TestContext ctx)
    {
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
    }
}
