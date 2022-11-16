using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletTrail : MonoBehaviour
{
    public abstract Vector3 StartPoint { set; }
    public abstract Vector3 EndPoint { set; }
    public float lifetime;
    protected float spawnTime;
    protected virtual void OnEnable() => spawnTime = Time.time;
}
