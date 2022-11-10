using UnityEngine;

public interface IAction
{
    public void Execute(GunData gunData, Vector3 direction, Vector3 position);
}
