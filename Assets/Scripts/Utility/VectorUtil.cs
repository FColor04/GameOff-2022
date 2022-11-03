using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class VectorUtil
{

    public static Vector3[] GetArcedPath(Vector3 start, Vector3 end, float height, float step = .1f)
    {
        var length = (start - end).magnitude;
        var steps = Mathf.CeilToInt(length / step);
        var arcPath = from i in Enumerable.Range(0, steps) let n = i / (steps - 1f) let x = n * 2 - 1 select start * (1 - n) + end * n + (1 - (x * x)) * Vector3.up * height;
        return arcPath.ToArray();
    }


    public static Vector3[] GetClosedCircleOutline(Vector3 center, float radius, int resolution) { var circle = GetCircleOutline(center, radius, resolution); return circle.Append(circle.FirstOrDefault()).ToArray(); }
    public static Vector3[] GetCircleOutline(Vector3 center, float radius, int resolution)
    {
        var points = (from i in Enumerable.Range(0, resolution) let angle = i * 2 * Mathf.PI / resolution select center + new Vector3(Mathf.Sin(angle) * radius, 0, Mathf.Cos(angle) * radius));
        return points.ToArray();
    }

    public static float PathLength(Vector3[] path) => path.Length > 1 ? (from i in Enumerable.Range(0, path.Length - 1) select (path[i] - path[i + 1]).magnitude).Sum() : 0f;

    public static Vector3[] ClampPathLength(Vector3[] path, float length)
    {
        if (path.Length < 2)
            return path;
        var budget = length;
        var checkPoints = new Queue<Vector3>(path);
        var clampedPath = new List<Vector3>();
        clampedPath.Add(checkPoints.Peek());
        while (budget > 0 && checkPoints.Count > 1)
        {
            var current = checkPoints.Dequeue();
            var next = checkPoints.Peek();
            var dist = (current - next).magnitude;
            if (dist > budget)
            {
                var t = budget / dist;
                var interpolated = Vector3.Lerp(current, next, t);
                clampedPath.Add(interpolated);
                break;
            }
            clampedPath.Add(next);
            budget -= dist;
        }
        return clampedPath.ToArray();
    }

    public static Vector2[] ClampPathLength(Vector2[] path, float length)
    {
        var budget = length;
        var checkPoints = new Queue<Vector2>(path);
        var clampedPath = new List<Vector2>();
        while (budget > 0)
        {
            var current = checkPoints.Dequeue();
            clampedPath.Add(current);
            var next = checkPoints.Peek();
            var dist = (current - next).magnitude;
            if (dist > budget)
            {
                var t = budget / dist;
                var interpolated = Vector3.Lerp(current, next, t);
                clampedPath.Add(interpolated);
                break;
            }
            budget -= dist;
        }
        return clampedPath.ToArray();
    }
}
