using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectUtility
{
    public static void SetLayerRecursive(this GameObject go, int layer)
    {
        var gos = new Queue<GameObject>();
        gos.Enqueue(go);
        while (gos.Count > 0)
        {
            go = gos.Dequeue();
            go.layer = layer;
            for (int i = 0; i < go.transform.childCount; i++)
                gos.Enqueue(go.transform.GetChild(i).gameObject);
        }
    }

    public static Bounds VisualBounds(this GameObject go)
    {
        var MeshRenderers = go.GetComponentsInChildren<MeshRenderer>();
        var bounds = MeshRenderers.Select(meshRenderer => meshRenderer.bounds);
        var combinedBounds = bounds.FirstOrDefault();
        foreach (var item in bounds.Skip(1))
            combinedBounds.Encapsulate(item);
        return combinedBounds;
    }
}
