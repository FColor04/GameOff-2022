using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    private GunInstance _equippedGun;
    public GunInstance equippedGun
    {
        get => _equippedGun;
        set => _equippedGun = value;
    }
}
