using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererBulletTrail : BulletTrail
{
    [SerializeField] private LineRenderer lr;

    public float Length { get => lr.positionCount >= 2 ? (lr.GetPosition(0) - lr.GetPosition(1)).magnitude : 0f; }

    public override Vector3 StartPoint { set { lr.SetPosition(0, value); lr.material.SetFloat("_LineLength", Length); } }
    public override Vector3 EndPoint { set { lr.SetPosition(1, value); lr.material.SetFloat("_LineLength", Length); } }
    protected override void OnEnable()
    {
        base.OnEnable();
        lr.positionCount = 2;
        lr.material.SetFloat("_SpawnTime", spawnTime);
    }

    void OnDisable()
    {
        lr.positionCount = 0;
    }

    void Update()
    {
        if (Time.time > spawnTime + lifetime) Destroy(gameObject);
    }
}
