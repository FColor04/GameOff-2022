using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
    public CrosshairData crosshairOverride;
    //FX
    [Header("Gun Fire Properties")]
    public float maxRange = 100f;
    public float damage = 2f;
    public bool isAutomatic;
    public float fireRate = 17;
    private float _fireTimer;
    [Header("Spread")]
    public float spreadSizePerShot = 0.2f;
    public float velocitySpreadMultiplier = 1f;
    public float spreadDecay = 0.2f;
    public float spreadMinSize = 0.1f;
    public float spreadMaxSize = 1.2f;
    [Header("OnFire")]
    [SerializeField, SerializeReference, HideInInspector] public List<IAction> actions;
}
