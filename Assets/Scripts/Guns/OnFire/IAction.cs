using System.Collections;
using UnityEngine;

public interface IAction
{
    public IEnumerator Execute(GunInstance gunInstance, Camera camera);
}
