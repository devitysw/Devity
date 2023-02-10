namespace Devity.Tests.Blazor;

public class NameTest
{
    protected const string DISPLAY_NAME = "DISPLAY_NAME";

    [Test]
    public void NameWithDisplay()
    {
        using var ctx = new Bunit.TestContext();

        var obj = new TestObject();
        var cut = ctx.RenderComponent<Name<string>>(parameters => parameters.Add(p => p.For, () => obj.Name));
        cut.MarkupMatches($"<label>{DISPLAY_NAME}</label>");
    }

    [Test]
    public void NameWithoutDisplay()
    {
        using var ctx = new Bunit.TestContext();

        var obj = new TestObject();
        var cut = ctx.RenderComponent<Name<string>>(parameters => parameters.Add(p => p.For, () => obj.WithoutDisplay));
        cut.MarkupMatches($"<label>WithoutDisplay</label>");
    }

    public class TestObject
    {
        [Display(Name = DISPLAY_NAME)]
        public string Name { get; set; } = string.Empty;

        public string WithoutDisplay { get; set; } = string.Empty;
    }
}
