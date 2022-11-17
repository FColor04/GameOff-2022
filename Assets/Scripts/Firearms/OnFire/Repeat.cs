using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firearms.Actions
{
    [System.Serializable]
    public class Repeat : IAction
    {
        public int repetitions;
        [SerializeReference] public List<IAction> actions;

        public IEnumerator Execute(FirearmInstance FirearmInstance, Camera camera)
        {
            for (int i = 0; i < repetitions; i++)
                foreach (var action in actions)
                {
                    if (action == null) continue;
                    var enumerator = action.Execute(FirearmInstance, camera);
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current == null) enumerator.MoveNext();
                        else yield return enumerator.Current;
                    }
                }
        }
    }
}