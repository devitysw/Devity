namespace Devity.Tests.Extensions;

public class NumberExtensionsTest
{
    [Test]
    public void ToHumanReadableSizeTest()
    {
        Assert.That(1450L.ToHumanReadableSize(), Is.EqualTo("1.42 KiB"));
        Assert.That(8546561561L.ToHumanReadableSize(), Is.EqualTo("7.96 GiB"));
        Assert.That(2980215156156L.ToHumanReadableSize(), Is.EqualTo("2.71 TiB"));
    }
}