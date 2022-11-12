using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpawnProjectile : IAction
{
    public GameObject prefab;
    
    public float spread;
    public float lifetime = 100f;
    public float launchSpeed = 20f;
    public float damage = 2f;

    public IEnumerator Execute(GunData gunData, Vector3 direction, Vector3 position)
    {
        yield return null;
    }
}
