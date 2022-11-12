using System.Collections;
using UnityEngine;

public class RaycastShoot : IAction
{
    public GameObject bulletTrail;
    public GameObject bulletImpact;
    public GameObject onHitVFX;

    public float spread;
    public float maxRange = 100f;
    public float damage = 2f;


    public IEnumerator Execute(GunData gunData, Vector3 direction, Vector3 position)
    {
        yield return null;
    }
}
