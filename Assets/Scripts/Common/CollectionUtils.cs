using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class CollectionUtils
{
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dict, K key) where V : new()
    {
        if (dict.ContainsKey(key)) return dict[key];
        V val = new();
        dict.Add(key, val);
        return val;
    }

    public static T Random<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static List<T> RandomPermutation<T>(this List<T> list)
    {        
        return list.OrderBy((t) => UnityEngine.Random.value).ToList();
    }
}
