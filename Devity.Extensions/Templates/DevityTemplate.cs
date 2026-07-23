namespace Devity.Extensions.Templates;

public class DevityTemplate
{
    public string BodyPath { get; }
    public Dictionary<string, string?> KeyMap { get; } = new();
    public Dictionary<string, string?> RawKeyMap { get; } = new();
    public Dictionary<string, dynamic> LoopMap { get; } = new();
    public Dictionary<string, bool> ConditionMap { get; } = new();

    public DevityTemplate(string bodyPath)
    {
        BodyPath = bodyPath;
    }

    /// <summary>Adds a key whose value is HTML-encoded before being inserted into the template. Use this for any value that isn't trusted, hand-authored markup (e.g. user-supplied text).</summary>
    public DevityTemplate AddKey(string key, string? value)
    {
        KeyMap.Add(key, value);
        return this;
    }

    /// <summary>Adds a key whose value is inserted into the template as raw, unescaped HTML. Only use this for trusted markup you authored yourself, never for user-supplied values.</summary>
    public DevityTemplate AddRawKey(string key, string? value)
    {
        RawKeyMap.Add(key, value);
        return this;
    }

    public DevityTemplate AddLoop(string key, dynamic loop)
    {
        LoopMap.Add(key, loop);
        return this;
    }

    public DevityTemplate AddCondition(string key, bool conditionResult)
    {
        ConditionMap.Add(key, conditionResult);
        return this;
    }
}
