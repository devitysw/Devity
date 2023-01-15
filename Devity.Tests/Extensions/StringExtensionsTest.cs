namespace Devity.Tests.Extensions;

public class StringExtensionsTest
{
    [Test]
    public void ShortenTest()
    {
        Assert.That("testing123".Shorten(4), Is.EqualTo("test..."));
        Assert.That("testing123".Shorten(20), Is.EqualTo("testing123"));
        Assert.That("".Shorten(4), Is.EqualTo(""));
        Assert.That("Hey, how are you doing?".Shorten(4), Is.EqualTo("Hey,..."));
    }
}