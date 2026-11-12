namespace Devity.Tests.Blazor;

public class ToastsTest
{
    private static readonly TimeSpan Wait = TimeSpan.FromSeconds(2);

    [Test]
    public async Task ShowsToastInDefaultStack()
    {
        using var ctx = new Bunit.TestContext();

        var cut = ctx.RenderComponent<Toasts>();
        await cut.InvokeAsync(() => cut.Instance.RunToast(new Toasts.Toast("Saved.", Toasts.ToastType.Success)));

        // NUnit records a failed Assert even when it's caught, so wait on a plain condition and assert afterwards.
        cut.WaitForState(() => cut.Markup.Contains("data-active=\"True\""), Wait);

        Assert.That(cut.Markup, Does.Contain("Saved."));
        Assert.That(cut.Find("div").ClassName, Does.Contain("dv:fixed"));

        var toast = cut.Find(".devity-toast");
        Assert.That(toast.GetAttribute("data-state"), Is.EqualTo(nameof(Toasts.ToastState.Shown)));
        Assert.That(toast.GetAttribute("style"), Does.Contain("--devity-toast-duration: 500ms"));
    }

    [Test]
    public async Task TemplateGetsBuiltInMotionAndSlidesOutWhenDismissed()
    {
        using var ctx = new Bunit.TestContext();
        Toasts.ToastContext? latest = null;

        var cut = ctx.RenderComponent<Toasts>(parameters => parameters
            .Add(p => p.LeaveDurationMilliseconds, 60_000)
            .Add(p => p.ToastTemplate, context =>
            {
                latest = context;
                return $"<p class=\"toast\">{context.Toast.Text}</p>";
            }));

        await cut.InvokeAsync(() => cut.Instance.RunToast(new Toasts.Toast("Hello", DurationMilliseconds: 60_000)));
        cut.WaitForState(() => latest?.State == Toasts.ToastState.Shown, Wait);

        await cut.InvokeAsync(() => latest!.Dismiss());
        cut.WaitForState(() => latest?.State == Toasts.ToastState.Leaving, Wait);

        var wrapper = cut.Find(".devity-toast");
        Assert.That(wrapper.GetAttribute("data-state"), Is.EqualTo(nameof(Toasts.ToastState.Leaving)));
        Assert.That(wrapper.QuerySelector("p.toast")?.TextContent, Is.EqualTo("Hello"));
    }

    [Test]
    public async Task TemplateRendersBareWhenAnimateTemplateIsOff()
    {
        using var ctx = new Bunit.TestContext();
        Toasts.ToastContext? latest = null;

        var cut = ctx.RenderComponent<Toasts>(parameters => parameters
            .Add(p => p.AnimateTemplate, false)
            .Add(p => p.ToastTemplate, context =>
            {
                latest = context;
                return $"<p class=\"toast\">{context.Toast.Text}</p>";
            }));

        await cut.InvokeAsync(() => cut.Instance.RunToast(new Toasts.Toast("Hello", DurationMilliseconds: 60_000)));
        cut.WaitForState(() => latest?.State == Toasts.ToastState.Shown, Wait);

        Assert.That(cut.FindAll(".devity-toast"), Is.Empty);
        Assert.That(cut.Find("div > p.toast").TextContent, Is.EqualTo("Hello"));
    }

    [Test]
    public void ContainerClassReplacesDefaultStack()
    {
        using var ctx = new Bunit.TestContext();

        var cut = ctx.RenderComponent<Toasts>(parameters => parameters
            .Add(p => p.ContainerClass, "contents"));

        Assert.That(cut.Find("div").ClassName, Is.EqualTo("contents"));
    }

    [Test]
    public async Task TemplateRendersEachToastAndCanDismissIt()
    {
        using var ctx = new Bunit.TestContext();
        Toasts.ToastContext? latest = null;

        var cut = ctx.RenderComponent<Toasts>(parameters => parameters
            .Add(p => p.LeaveDurationMilliseconds, 0)
            .Add(p => p.ToastTemplate, context =>
            {
                latest = context;
                return $"<p class=\"toast\" data-state=\"{context.State}\">{context.Toast.Text}</p>";
            }));

        await cut.InvokeAsync(() => cut.Instance.RunToast(new Toasts.Toast("Hello", DurationMilliseconds: 60_000)));

        cut.WaitForState(() => latest?.State == Toasts.ToastState.Shown, Wait);

        var toast = cut.Find("p.toast");
        Assert.That(toast.TextContent, Is.EqualTo("Hello"));
        Assert.That(toast.GetAttribute("data-state"), Is.EqualTo(nameof(Toasts.ToastState.Shown)));
        Assert.That(cut.Markup, Does.Not.Contain("data-active"));

        await cut.InvokeAsync(() => latest!.Dismiss());

        cut.WaitForState(() => cut.FindAll("p.toast").Count == 0, Wait);
    }
}
