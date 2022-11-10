using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class GunInstance : MonoBehaviour, IInteractable
{
    [Header("GunModel References")]
    public Transform muzzle;
    public GameObject hole;
    public string Message => $"Pick up {gunData.name}";
    [SerializeField] private GunData gunData;
    public GunData GunData { get => gunData; }
    public VisualEffect droppedStateVfx;

    private Collider m_collider;
    private Rigidbody m_rigidbody;


    public void Execute(PlayerInteraction sender)
    {
        var equipment = sender.GetComponent<PlayerEquipment>();
        equipment.equippedGun = this;
        this.OnPickup();
    }

    public void OnPickup()
    {
        m_collider.enabled = false;
        m_rigidbody.isKinematic = true;
        transform.localRotation = Quaternion.identity;
    }

    public void OnDrop(Vector3 force)
    {
        m_collider.enabled = true;
        m_rigidbody.isKinematic = false;
        m_rigidbody.velocity = force;
    }

    public void NotifyLookedAt()
    {
        Debug.Log($"started looking at {name}");
    }

    public void NotifyLookedAway()
    {
        Debug.Log($"stopped looking at {name}");
    }
}
