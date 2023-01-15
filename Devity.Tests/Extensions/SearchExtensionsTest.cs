namespace Devity.Tests.Extensions;

public class SearchExtensionsTest
{
    [Test]
    public void HasTest()
    {
        Assert.That("testing123".Has("test"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("ahojakosamas"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("akosamas"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("akosamas?"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has(",akosamas?"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("Ahoj"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("ahoj"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("ako sa máš"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("máš?"), Is.True);
        Assert.That("Ahoj, ako sa máš?".Has("máeš?"), Is.False);
        Assert.That("Ahoj, ako sa máš?".Has("bla"), Is.False);
        Assert.That("".Has("e"), Is.False);
    }
}