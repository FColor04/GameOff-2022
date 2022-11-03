using System.Collections.Generic;

public static class CollectionUtils
{
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key) where V : new()
    {
        if (dict.ContainsKey(key)) return dict[key];
        V val = new();
        dict.Add(key, val);
        return val;
    }
}
