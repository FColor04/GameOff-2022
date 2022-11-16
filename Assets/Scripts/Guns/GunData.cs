using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public CrosshairData crosshairOverride;
    
    public float fireRate = 17;
    public bool isAutomatic;
    public RuntimeAnimatorController animationSet;
    [Header("OnPrimaryFire")]
    [SerializeField, SerializeReference, HideInInspector] public List<IAction> onPrimaryFireActions;
    [SerializeField, SerializeReference, HideInInspector] public List<IAction> onSecondaryFireActions;
}
