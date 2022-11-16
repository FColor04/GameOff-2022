using System.Collections;
using System.Linq;
using UnityEngine;

public class RaycastShoot : IAction
{
    public BulletTrail bulletTrail;
    public GameObject impactDecal;
    public GameObject hitVFX;

    public float spread;
    public float maxRange = 100f;
    public float damage = 2f;
    public int piercingCount = 0;


    public IEnumerator Execute(GunInstance gunInstance, Camera camera)
    {
        var halfRadSpread = spread * Mathf.PI / 360f;
        var offset = Random.insideUnitCircle._xy0();
        var ray = camera.ViewportPointToRay(Vector3.one * .5f);
        var direction = Quaternion.LookRotation(ray.direction, Vector3.up) * (Vector3.forward * (1 - halfRadSpread) + offset * halfRadSpread).normalized;
        var hits = Physics.RaycastAll(ray.origin, direction, maxRange, ~(1 << LayerMask.NameToLayer("Player")));
        if (piercingCount >= 0) hits = hits.OrderBy(hit => hit.distance).Take(piercingCount + 1).ToArray(); //Only take closest <piercingCount> + 1 hits

        foreach (var hit in hits)
        {
            Debug.Log($"hit: {hit.collider.gameObject.name} at {hit.point}");
            //Spawn Hit VFX
        }
        //Spawn Trail VFX     
        if (bulletTrail)
        {
            var startPosition = gunInstance.muzzlePositionTransform.position.TransformFOV(camera, 35f);
            var furthestHitPoint = hits.Length > 0 ? hits.Last().point : ray.origin + direction * maxRange;
            var bulletTrailInstance = GameObject.Instantiate(bulletTrail);
            bulletTrailInstance.Lifetime = .35f;
            bulletTrailInstance.StartPoint = startPosition;
            bulletTrailInstance.EndPoint = furthestHitPoint;
        }
        yield return null;
    }
}
