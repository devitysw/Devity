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

    [Test]
    public void ToFormattedIbanTest()
    {
        Assert.That("SK3112000000001987426375".ToFormattedIban(), Is.EqualTo("SK31 1200 0000 0019 8742 6375"));
        Assert.That("SK6807200002891987426353".ToFormattedIban(), Is.EqualTo("SK68 0720 0002 8919 8742 6353"));
        Assert.That("SK68072000028919874263".ToFormattedIban(), Is.EqualTo("SK68 0720 0002 8919 8742 63"));
        Assert.That("test".ToFormattedIban(), Is.EqualTo("test"));
    }
}