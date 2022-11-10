using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    public Transform gunAnchor;
    public Transform gunAnchorDownsights;
    private GunInstance _equippedGun;
    public GunInstance equippedGun
    {
        get => _equippedGun;
        set => _equippedGun = value;
    }
    public float smoothTime;
    public CrosshairRenderer crosshair;
    public PlayerCamera playerCamera;
    private Vector3 gunPosition;
    public bool aimingDownsights;

    private void Start()
    {
        gunPosition = gunAnchor.localPosition;
        playerCamera = GetComponent<PlayerCamera>();
    }

    private void Update()
    {
        gunPosition = Vector3.Lerp(gunPosition, aimingDownsights ? gunAnchorDownsights.localPosition : gunAnchor.localPosition, smoothTime * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (equippedGun != null)
        {
            equippedGun.transform.localPosition = gunPosition;
        }
    }

    public void OnPlayerShoot()
    {
        crosshair.CurrentCrosshair.runtimeOutsideBehaviourAmount += 1;
        crosshair.CurrentCrosshair.runtimeOutsideBehaviourAmount = Mathf.Clamp01(crosshair.CurrentCrosshair.runtimeOutsideBehaviourAmount);
    }
}
