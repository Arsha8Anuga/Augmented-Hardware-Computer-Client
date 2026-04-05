using System.Collections.Generic;

public class DataCache<T>
{
    private Dictionary<string, T> cache = new();

    public void Set(string id, T data)
    {
        cache[id] = data;
    }
    
    public T Get(string id)
    {
        cache.TryGetValue(id, out var data);
        return data;
    }

    public void Clear()
    {
        cache.Clear();
    }

    public Dictionary<string, T> GetAll()
    {
        return new Dictionary<string, T>(cache);
    }

    public bool HasData()
    {
        return cache.Count > 0;
    }
}