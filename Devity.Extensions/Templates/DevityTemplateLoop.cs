using System.Linq.Expressions;

namespace Devity.Extensions.Templates;

public class DevityTemplateLoop<T>
{
    public List<T> Objects { get; }
    public Dictionary<string, Expression<Func<T, dynamic>>> KeyMap { get; } = new();
    public Dictionary<string, Expression<Func<T, dynamic>>> RawKeyMap { get; } = new();

    public DevityTemplateLoop(List<T> objects)
    {
        Objects = objects;
    }

    /// <summary>Adds a key whose value is HTML-encoded before being inserted into the template. Use this for any value that isn't trusted, hand-authored markup (e.g. user-supplied text).</summary>
    public DevityTemplateLoop<T> AddKey(string key, Expression<Func<T, dynamic>> value)
    {
        KeyMap.Add(key, value);
        return this;
    }

    /// <summary>Adds a key whose value is inserted into the template as raw, unescaped HTML. Only use this for trusted markup you authored yourself, never for user-supplied values.</summary>
    public DevityTemplateLoop<T> AddRawKey(string key, Expression<Func<T, dynamic>> value)
    {
        RawKeyMap.Add(key, value);
        return this;
    }
}
