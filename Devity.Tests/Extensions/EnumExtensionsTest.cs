namespace Devity.Tests.Extensions;

public class EnumExtensionsTest
{
    private const string VALUE = "Test value 1";

    public enum Test
    {
        [Display(Name = VALUE)]
        TestValue1,
        TestValue2
    }

    [Test]
    public void GetDisplayNameTest()
    {
        Assert.That(Test.TestValue1.GetDisplayName(), Is.EqualTo(VALUE));
        Assert.That(Test.TestValue2.GetDisplayName(), Is.EqualTo("TestValue2"));
    }
}