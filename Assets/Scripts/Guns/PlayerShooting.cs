using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerEquipment equipment;
    [SerializeField] private PlayerCamera playerCamera;
    private float lastShotFired;
    private bool currentlyShooting;
    private bool holdingPrimaryFireButton, holdingSecondaryFireButton;
    private float lastButtonRelease;
    private GunData GunData { get => equipment?.EquippedGun?.GunData; }
    public GunAnimations gunAnimations;
    public void Update()
    {
        if (holdingPrimaryFireButton && GunData && (GunData.isAutomatic || lastButtonRelease > lastShotFired))
            FirePrimary();
        if (holdingSecondaryFireButton && GunData && (GunData.isAutomatic || lastButtonRelease > lastShotFired))
            FireSecondary();
    }

    public void OnPrimaryFireButtonPressed(InputAction.CallbackContext callback)
    {
        holdingPrimaryFireButton = true;
        if (!equipment || !equipment.EquippedGun || !GunData) return;
        FirePrimary();
    }
    public void OnPrimaryFireButtonReleased(InputAction.CallbackContext callback)
    {
        holdingPrimaryFireButton = false;
        lastButtonRelease = Time.time;
    }

    private void FirePrimary()
    {
        if (currentlyShooting || !GunData || Time.time < lastShotFired + 1f / GunData.fireRate) return;
        currentlyShooting = true;
        StartCoroutine(ExecuteActions(GunData.onPrimaryFireActions));
    }

    private IEnumerator ExecuteActions(List<IAction> actions)
    {
        foreach (var action in actions)
        {
            if (action == null) continue;
            var enumerator = action.Execute(equipment.EquippedGun, playerCamera.Camera, gunAnimations);
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == null) enumerator.MoveNext();
                else yield return enumerator.Current;
            }
        }
        currentlyShooting = false;
        lastShotFired = Time.time;
    }

    private void FireSecondary()
    {
        if (currentlyShooting || !GunData || Time.time < lastShotFired + 1f / GunData.fireRate) return;
        currentlyShooting = true;
        StartCoroutine(ExecuteActions(GunData.onSecondaryFireActions));
    }


    #region Secondary Fire
    public void OnSecondaryFireButtonPressed(InputAction.CallbackContext callback)
    {
        holdingSecondaryFireButton = true;
        if (!equipment || !equipment.EquippedGun || !GunData) return;        
        FireSecondary();
    }
    public void OnSecondaryFireButtonReleased(InputAction.CallbackContext callback)
    {
        holdingSecondaryFireButton = false;
    }

    #endregion
}
