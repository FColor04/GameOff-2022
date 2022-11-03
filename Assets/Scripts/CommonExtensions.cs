using System.Collections.Generic;

public static class CommonExtensions
{
    public static T Random<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}