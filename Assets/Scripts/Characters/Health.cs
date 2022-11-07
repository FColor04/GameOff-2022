using System;

[Serializable]
public struct Health
{
    public float current;
    public float min;
    public float max;

    public Health(float max)
    {
        current = max;
        min = 0f;
        this.max = max;
    }
}