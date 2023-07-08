namespace Devity.Tests.Extensions;

public class QueryableExtensionsTest
{
    private const string VALUE_1 = "Test1";
    private const string VALUE_2 = "Test2";

    private class TestClass
    {
        public string? TestValue { get; set; }
    }

    [Test]
    public void DistinctByDbTest()
    {
        var list = new List<TestClass>
        {
            new TestClass { TestValue = VALUE_1 },
            new TestClass { TestValue = VALUE_2 },
            new TestClass { TestValue = VALUE_1 },
            new TestClass { TestValue = null }
        }.AsQueryable();

        var result = list.DistinctByDb(x => x.TestValue).ToList();

        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result.ElementAt(0).TestValue, Is.EqualTo(VALUE_1));
        Assert.That(result.ElementAt(1).TestValue, Is.EqualTo(VALUE_2));
    }
}
