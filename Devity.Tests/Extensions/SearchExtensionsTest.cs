namespace Devity.Tests.Extensions;

public class SearchExtensionsTest
{
    [Test]
    public void HasTest()
    {
        Assert.That("testing123".Has("test"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("ahojakosamas"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("akosamas"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("akosamas?"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has(",akosamas?"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("Ahoj"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("ahoj"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("ako sa m�"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("m�?"), Is.True);
        Assert.That("Ahoj, ako sa m�?".Has("m�e�?"), Is.False);
        Assert.That("Ahoj, ako sa m�?".Has("bla"), Is.False);
        Assert.That("".Has("e"), Is.False);
    }
}