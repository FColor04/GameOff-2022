using UnityEngine;

[System.Serializable]
public class SpawnProjectile : IAction
{
    public GameObject prefab;

    public void Execute(GunData gunData, Vector3 direction, Vector3 position)
    {
        throw new System.NotImplementedException();
    }
}
