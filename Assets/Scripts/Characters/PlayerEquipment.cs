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
    private Gun _equippedGun;
    public Gun equippedGun
    {
        get => _equippedGun;
        set
        {
            if (_equippedGun != null)
            {
                equippedGun.OnDrop(gunAnchor.forward * 8f);
                equippedGun.transform.SetParent(null);
                equippedGun.equipped = false;
                _equippedGun.OnGunShoot -= OnPlayerShoot;
            }

            _equippedGun = value;
            crosshair.currentCrosshair = null;
            
            if (_equippedGun != null)
            {
                crosshair.currentCrosshair = _equippedGun.crosshairOverride;
                _equippedGun.OnGunShoot += OnPlayerShoot;
                _equippedGun.transform.SetParent(playerCamera.Camera);
                _equippedGun.OnPickup();
            }
        }
    }
    public float smoothTime;
    public Crosshair crosshair;
    public TMP_Text pickupText;
    public PlayerCamera playerCamera;
    private Vector3 gunPosition;
    public bool aimingDownsights;

    private void Start()
    {
        gunPosition = gunAnchor.localPosition;
        equippedGun = GetComponentInChildren<Gun>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    private void Update()
    {
        aimingDownsights = Mouse.current.rightButton.isPressed;
        gunPosition = Vector3.Lerp(gunPosition, aimingDownsights ? gunAnchorDownsights.localPosition : gunAnchor.localPosition, smoothTime * Time.deltaTime);

        if (Keyboard.current.gKey.wasPressedThisFrame && equippedGun != null)
        {
            equippedGun = null;
        }

        if (Physics.Raycast(playerCamera.Camera.position, playerCamera.Camera.forward, out var hit, 4f))
        {
            pickupText.gameObject.SetActive(false);
            if (hit.transform.TryGetComponent(out Gun gunComponent))
            {
                pickupText.gameObject.SetActive(true);
                pickupText.text = $"Press [E] to pickup {hit.transform.name}";

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    equippedGun = gunComponent;
                }
            }
        }
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
