using System.Collections;
using UnityEngine;

public interface IAction
{
    public IEnumerator Execute(GunData gunData, Vector3 direction, Vector3 position);
}
