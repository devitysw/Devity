namespace Devity.Tests.Extensions.Helpers;

public class ClassFacadeTest
{
    private const string VALUE = "Test value 1";

    private class TestObject
    {
        [Display(Name = VALUE)]
        public string Value { get; set; } = default!;

        public string NoDisplayName { get; set; } = default!;
    }

    private class TestGenericObject<T> where T : TestObject
    {
    }

    private TestObject _object = new TestObject();

    [Test]
    public void GetCleanTypeTest()
    {
        Assert.That(ClassFacade.GetCleanType(_object), Is.EqualTo("TestObject"));
        Assert.That(ClassFacade.GetCleanType(new TestGenericObject<TestObject>()), Is.EqualTo("TestGenericObject-TestObject"));
    }

    [Test]
    public void GetPropertyHumanNameTest()
    {
        var type = typeof(TestObject);
        Assert.That(ClassFacade.GetPropertyHumanName(type.GetProperty("Value")!), Is.EqualTo(VALUE));
        Assert.That(ClassFacade.GetPropertyHumanName(type.GetProperty("NoDisplayName")!), Is.EqualTo("NoDisplayName"));
    }
}