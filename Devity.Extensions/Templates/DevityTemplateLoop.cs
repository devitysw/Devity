using System.Linq.Expressions;

namespace Devity.Extensions.Templates;

public class DevityTemplateLoop<T>
{
    public List<T> Objects { get; }
    public Dictionary<string, Expression<Func<T, dynamic>>> KeyMap { get; } = new();

    public DevityTemplateLoop(List<T> objects)
    {
        Objects = objects;
    }

    public DevityTemplateLoop<T> AddKey(string key, Expression<Func<T, dynamic>> value)
    {
        KeyMap.Add(key, value);
        return this;
    }
}
