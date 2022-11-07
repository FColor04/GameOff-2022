using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[ExecuteAlways]
public class CrouchController : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint spineJoint;
    [SerializeField] private Rigidbody bodyRB;
    [SerializeField] private Rigidbody headRB;
    [SerializeField] private CapsuleCollider headCollider;
    [SerializeField] private CapsuleCollider legsCollider;

    public float antiDecapitationLimit = .5f;
    public float crouchLen;
    public float standingLen;
    private bool crouching = false;
    public bool Crouching
    {
        get => crouching;
        set
        {
            if (crouching == value) return;
            crouching = value;
            headRB.WakeUp();
        }
    }

    void OnValidate()
    {
        var spineLen = crouching ? crouchLen : standingLen;
        headRB.transform.localPosition = Vector3.up * spineLen;
        legsCollider.height = crouchLen + headCollider.radius;
        legsCollider.center = Vector3.up * legsCollider.height / 2f;
        headCollider.height = standingLen - crouchLen + headCollider.radius * 2f;
        headCollider.center = headCollider.center._x0z() - Vector3.up * (headCollider.height / 2f - headCollider.radius);
    }

    void OnEnable() => headRB.sleepThreshold = -1f;
    void OnDisable() => headRB.sleepThreshold = 0.005f;

    void FixedUpdate()
    {
        headRB.transform.localPosition = headRB.transform.localPosition._0y0();
        var spineLen = crouching ? crouchLen : standingLen;        
        spineJoint.connectedAnchor = transform.TransformPoint(Vector3.up * spineLen);

        var isWorriedAboutDecapitation = headRB.transform.localPosition.y > spineLen + antiDecapitationLimit;        
        headCollider.enabled = !isWorriedAboutDecapitation;
        if (bodyRB.velocity.y > 0) headRB.velocity = Vector3.up * bodyRB.velocity.y;
    }
}
