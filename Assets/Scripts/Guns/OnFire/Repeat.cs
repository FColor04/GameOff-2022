using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Repeat : IAction
{
    public int repetitions;
    [SerializeReference] public List<IAction> actions;

    public IEnumerator Execute(GunData gunData, Vector3 direction, Vector3 position)
    {
        for(int i = 0; i < repetitions; i++)
            actions.ForEach(action => action.Execute(gunData, direction, position));
        yield return null;
    }
}
