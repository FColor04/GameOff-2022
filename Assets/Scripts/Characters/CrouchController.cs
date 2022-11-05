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
    [SerializeField] private CapsuleCollider spineCollider;
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
        spineCollider.height = crouchLen + spineCollider.radius / 2f;
        spineCollider.center = spineCollider.center._x0z() - Vector3.up * (crouchLen / 2f + spineCollider.radius);
    }

    void OnEnable() => headRB.sleepThreshold = -1f;
    void OnDisable() => headRB.sleepThreshold = 0.005f;

    void FixedUpdate()
    {
        headRB.transform.localPosition = headRB.transform.localPosition._0y0();
        var spineLen = crouching ? crouchLen : standingLen;
        spineJoint.connectedAnchor = transform.TransformPoint(Vector3.up * spineLen);
        if(bodyRB.velocity.y > 0) headRB.velocity = Vector3.up * bodyRB.velocity.y;

    }
}
