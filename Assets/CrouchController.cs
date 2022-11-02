using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchController : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint spine;
    [SerializeField] private Rigidbody headRB;
    public float crouchLen;
    public float standingLen;
    bool crouching = false;

    void OnEnable()
    {
        headRB.sleepThreshold = -1f;
    }

    void Update()
    {
        crouching = Input.GetKeyDown(KeyCode.LeftShift) ? !crouching : crouching;
    }

    void FixedUpdate()
    {
        var spineLen = crouching ? crouchLen : standingLen;
        spine.connectedAnchor = transform.TransformPoint(Vector3.up * spineLen);
    }
}
