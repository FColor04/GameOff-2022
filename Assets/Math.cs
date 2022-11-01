using System;

public static class Math
{
    public static float sMin(float a, float b, float k)
    {
        float h = MathF.Max(k - MathF.Abs(a - b), 0.0f) / k;
        return MathF.Min(a, b) - h * h * k * (.25f);
    }
}