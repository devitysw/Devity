namespace Devity.Extensions.Templates;

public class DevityTemplate
{
    public string BodyPath { get; }
    public Dictionary<string, string> KeyMap { get; } = new();
    public Dictionary<string, dynamic> LoopMap { get; } = new();

    public DevityTemplate(string bodyPath)
    {
        BodyPath = bodyPath;
    }

    public DevityTemplate AddKey(string key, string value)
    {
        KeyMap.Add(key, value);
        return this;
    }

    public DevityTemplate AddLoop(string key, dynamic loop)
    {
        LoopMap.Add(key, loop);
        return this;
    }
}
