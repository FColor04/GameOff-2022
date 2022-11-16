using System.Collections;
using UnityEngine;

[System.Serializable]
public class Wait : IAction
{
    public float duration = .1f;
    public IEnumerator Execute(GunInstance gunInstance, Camera camera, GunAnimations animations)
    {
        yield return new WaitForSeconds(duration);
    }
}
