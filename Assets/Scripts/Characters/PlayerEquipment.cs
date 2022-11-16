using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEquipment : MonoBehaviour
{
    public Transform GunAnchor;
    private GunInstance m_equippedGun;
    public GunInstance EquippedGun
    {
        get => m_equippedGun; set
        {
            if (m_equippedGun == value) return;
            if (m_equippedGun)
            {
                m_equippedGun.transform.SetParent(null);
                m_equippedGun.transform.position = transform.position + Vector3.up * 1.5f;
                m_equippedGun.transform.localRotation = Quaternion.identity;
                m_equippedGun.transform.localScale = Vector3.one;
                m_equippedGun.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Guns"));
            }
            m_equippedGun = value;
            if (!m_equippedGun) return;
            m_equippedGun.transform.SetParent(GunAnchor);
            m_equippedGun.transform.localPosition = Vector3.zero;
            m_equippedGun.transform.localRotation = Quaternion.identity;
            m_equippedGun.transform.localScale = Vector3.one;
            m_equippedGun.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Player"));
        }
    }

    void LateUpdate()
    {
        if (!EquippedGun) return;

    }
}
