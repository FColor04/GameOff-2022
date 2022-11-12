using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wait : IAction
{
    public float duration = .1f;
    public IEnumerator Execute(GunData gunData, Vector3 direction, Vector3 position)
    {
        yield return new WaitForSeconds(duration);
    }
}
