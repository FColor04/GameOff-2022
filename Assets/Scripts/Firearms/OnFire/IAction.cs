using System.Collections;
using UnityEngine;

namespace Firearms.Actions
{
    public interface IAction
    {
        public IEnumerator Execute(FirearmInstance FirearmInstance, Camera camera);
    }
}