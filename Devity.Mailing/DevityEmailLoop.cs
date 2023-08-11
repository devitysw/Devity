using System.Linq.Expressions;

namespace Devity.Mailing;

public class DevityEmailLoop<T>
{
    public List<T> Objects { get; }
    public Dictionary<string, Expression<Func<T, dynamic>>> KeyMap { get; } = new();

    public DevityEmailLoop(List<T> objects)
    {
        Objects = objects;
    }

    public DevityEmailLoop<T> AddKey(string key, Expression<Func<T, dynamic>> value)
    {
        KeyMap.Add(key, value);
        return this;
    }
}