using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class GunInstance : MonoBehaviour, IInteractable
{
    [Header("GunModel References")]
    public GameObject Model;
    public Transform muzzlePositionTransform;
    public string Message => $"Pick up {gunData.name}";
    [SerializeField] private GunData gunData;
    public GunData GunData { get => gunData; }
    public VisualEffect droppedStateVfx;

    private Collider m_collider;
    private Collider Collider { get => m_collider ??= GetComponent<Collider>(); }
    private Rigidbody m_rigidbody;
    private Rigidbody Rigidbody { get => m_rigidbody ??= GetComponent<Rigidbody>(); }


    public void Execute(PlayerInteraction sender)
    {
        var equipment = sender.GetComponent<PlayerEquipment>();
        var prevGun = equipment.EquippedGun;
        equipment.EquippedGun = this;
        prevGun?.OnDrop(sender?.GetComponent<PlayerCamera>()?.CameraRoot?.forward * 15f ?? Vector3.up * 15f);
        this.OnPickup();
    }

    public void OnEnable() { if (Model) Model.transform.localPosition = -(Model.VisualBounds().center - Model.transform.position); }

    public void OnPickup()
    {
        Model.transform.localPosition = Vector3.zero;        
        Collider.enabled = false;
        Rigidbody.isKinematic = true;
        Rigidbody.interpolation = RigidbodyInterpolation.None;
        Rigidbody.detectCollisions = false;
        transform.localRotation = Quaternion.identity;
        droppedStateVfx.enabled = false;
    }

    public void OnDrop(Vector3 force)
    {
        Model.transform.localPosition = -(Model.VisualBounds().center - Model.transform.position);
        Collider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Rigidbody.detectCollisions = true;
        Rigidbody.velocity = force;
        droppedStateVfx.enabled = true;
    }

    public void NotifyLookedAt()
    {

    }

    public void NotifyLookedAway()
    {

    }
}
