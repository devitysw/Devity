namespace Devity.Tests.Extensions;

public class DateTimeExtensionsTest
{
    [Test]
    public void DateToHtmlStringTest()
    {
        Assert.That(new DateTime(2001, 12, 20).DateToHtmlString(), Is.EqualTo("2001-12-20"));
        Assert.That(new DateTime(2001, 12, 20, 1, 1, 1).DateToHtmlString(), Is.EqualTo("2001-12-20"));
        Assert.That(new DateTime(2099, 1, 1, 1, 1, 1).DateToHtmlString(), Is.EqualTo("2099-01-01"));
    }
}