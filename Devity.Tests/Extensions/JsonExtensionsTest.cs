namespace Devity.Tests.Extensions;

public class JsonExtensionsTest
{
    [Test]
    public void ToJsonTest()
    {
        object testObject = new { Text = "bla", Number = 1 };
        Assert.That(
            testObject.ToJson(),
            Is.EqualTo("{" + "\r\n  \"Text\": \"bla\",\r\n  \"Number\": 1\r\n" + "}")
        );
    }

    [Test]
    public void ToJsonNoIndentTest()
    {
        object testObject = new { Text = "bla", Number = 1 };
        Assert.That(
            testObject.ToJson(false),
            Is.EqualTo("{" + "\"Text\":\"bla\",\"Number\":1" + "}")
        );
    }
}
