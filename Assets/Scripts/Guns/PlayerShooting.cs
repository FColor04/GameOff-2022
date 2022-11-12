using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private PlayerEquipment equipment;
    private float lastShotFired;
    private GunData GunData { get => equipment?.equippedGun?.GunData; }

    public void OnPrimaryFireButtonPressed()
    {
        if (!equipment || !equipment.equippedGun || !GunData) return;
        var gunInstance = equipment.equippedGun;
        FirePrimary();
    }
    public void OnPrimaryFireButtonHeld()
    {
        if (!GunData.isAutomatic) return;
        FirePrimary();
    }
    public void OnPrimaryFireButtonReleased()
    {

    }

    private void FirePrimary()
    {
        if (!GunData || Time.time < lastShotFired + 1f / GunData.fireRate) return;

    }


    #region Secondary Fire
    public void OnSecondaryFireButtonPressed()
    {

    }
    public void OnSecondaryFireButtonHeld()
    {

    }
    public void OnSecondaryFireButtonReleased()
    {

    }

    private void FireSecondary()
    {
        
    }
    #endregion
}
