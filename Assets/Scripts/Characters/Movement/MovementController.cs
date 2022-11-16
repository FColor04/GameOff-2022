using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private VelocityController vc;

    public bool Grounded { get => activeContacts.Count > 0; }
    private Dictionary<Collider, Vector3> activeContacts = new();

    public Vector3 GroundNormal { get => activeContacts.Count > 0 ? (activeContacts.Values.Aggregate((x, y) => x + y)).normalized : Vector3.up; }

    private float m_GroundSlopeThreshold = 35f / 180f * Mathf.PI;
    public float GroundSlopeThreshold { get => m_GroundSlopeThreshold * 180f / Mathf.PI; set => m_GroundSlopeThreshold = value / 180f * Mathf.PI; }

    public Vector2 XZPlaneMovement { get; set; }

    void Start() => StartCoroutine(PostFixedUpdate());

    IEnumerator PostFixedUpdate() //this Coroutine ALWAYS runs AFTER FixedUpdate, PhysicsUpdate and OnCollisionXXX callbacks
    {
        var afterFixedUpdateYielder = new WaitForFixedUpdate();
        while (true)
        {
            var clearChannelMask = Grounded ? VelocityChannelMask.XYZ : VelocityChannelMask.XZ;
            vc.AddOverwriteMovement(new(-GroundNormal, 1f, VelocityBlendMode.Overwrite, clearChannelMask), 0f, -100);
            yield return afterFixedUpdateYielder;
        }
    }

    void FixedUpdate()
    {
        if (XZPlaneMovement.sqrMagnitude <= float.Epsilon) return;
        var floorParallelRotation = Grounded ? Quaternion.FromToRotation(Vector3.up, GroundNormal) : Quaternion.identity;
        var floorParallelMovement = floorParallelRotation * XZPlaneMovement._x0y();
        var rotationXZMagnitudeLossFactor = XZPlaneMovement.magnitude / floorParallelMovement._x0z().magnitude;
        var yChannelNormalizationLossFactor = floorParallelMovement.magnitude / floorParallelMovement._x0z().magnitude;        
        var speed = floorParallelMovement.magnitude * rotationXZMagnitudeLossFactor * yChannelNormalizationLossFactor;
        vc.AddOverwriteMovement(new(floorParallelMovement, speed, VelocityBlendMode.Overwrite, VelocityChannelMask.XZ), 0f, 0);
    }

    public void OnCollisionStay(Collision collision)
    {
        var contactPoint = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoint);
        var averagedGroundNormal = Vector3.zero;
        var hasGroundContacts = false;
        foreach (var contact in contactPoint)
        {
            var angle = Mathf.Acos(contact.normal.normalized.y);
            var angleDiffToGroundPlane = Mathf.Abs(angle);
            if (angleDiffToGroundPlane > m_GroundSlopeThreshold) continue;
            averagedGroundNormal += contact.normal;
            hasGroundContacts = true;
        }
        if (hasGroundContacts)
            activeContacts[collision.collider] = averagedGroundNormal;
        if (Grounded) rb.useGravity = false;
    }

    public void OnCollisionExit(Collision collision)
    {
        activeContacts.Remove(collision.collider);
        if (!Grounded) rb.useGravity = true;
    }

    public void OnDrawGizmosSelected()
    {
        var floorParallelRotation = Grounded ? Quaternion.FromToRotation(Vector3.up, GroundNormal) : Quaternion.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + floorParallelRotation * Vector3.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + floorParallelRotation * Vector3.right);
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, transform.position + floorParallelRotation * Vector3.back);
        Gizmos.DrawLine(transform.position, transform.position + floorParallelRotation * Vector3.left);
        Gizmos.color = Color.white;
    }
}
