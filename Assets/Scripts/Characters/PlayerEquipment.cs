using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Transform gunAnchor;
    private Gun _equippedGun;
    public Gun equippedGun
    {
        get => _equippedGun;
        set
        {
            if(_equippedGun != null)
                _equippedGun.OnGunShoot -= OnPlayerShoot;
            _equippedGun = value;
            _equippedGun.OnGunShoot += OnPlayerShoot;
        }
    }
    public float smoothTime;
    public Crosshair crosshair;
    
    private Vector3 gunPosition;
    
    private void Awake()
    {
        gunPosition = gunAnchor.position;
        equippedGun = GetComponentInChildren<Gun>();
    }

    private void Update()
    {
        gunPosition = Vector3.Lerp(gunPosition, gunAnchor.position, smoothTime * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (equippedGun != null)
        {
            equippedGun.transform.position = gunPosition;
        }
    }

    public void OnPlayerShoot()
    {
        crosshair.outsideBehaviourAmount += 1;
        crosshair.outsideBehaviourAmount = Mathf.Clamp01(crosshair.outsideBehaviourAmount);
    }
}
