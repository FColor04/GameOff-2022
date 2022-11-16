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

    public IEnumerator Execute(GunInstance gunInstance, Camera camera)
    {
        yield return null;
    }
}
