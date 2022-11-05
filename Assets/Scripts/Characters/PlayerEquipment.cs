using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Transform gunAnchor;
    public Gun equippedGun;
    public float smoothTime;

    private Vector3 gunPosition;
    
    private void Awake()
    {
        gunPosition = gunAnchor.position;
    }

    private void Update()
    {
        gunPosition = Vector3.Lerp(gunPosition, gunAnchor.position, Time.deltaTime * smoothTime);
    }

    private void LateUpdate()
    {
        if (equippedGun != null)
        {
            equippedGun.transform.position = gunPosition;
        }
    }
}
