using System.Collections.Generic;
using UnityEngine;
using Firearms.Actions;


namespace Firearms
{
    [CreateAssetMenu]
    public class FirearmData : ScriptableObject
    {
        public CrosshairData crosshairOverride;

        public float fireRate = 17;
        public bool isAutomatic;
        [Header("OnPrimaryFire")]
        [SerializeField, SerializeReference, HideInInspector] public List<IAction> onPrimaryFireActions;
        [SerializeField, SerializeReference, HideInInspector] public List<IAction> onSecondaryFireActions;
    }
}