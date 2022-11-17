using System.Collections;
using UnityEngine;

namespace Firearms.Actions
{
    [System.Serializable]
    public class Wait : IAction
    {
        public float duration = .1f;
        public IEnumerator Execute(FirearmInstance FirearmInstance, Camera camera)
        {
            yield return new WaitForSeconds(duration);
        }
    }
}