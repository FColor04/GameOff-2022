using UnityEngine;

public class FOVTransform : MonoBehaviour
{
    private Camera cached_Camera;
    private Camera Camera { get => cached_Camera ??= GetComponentInParent<Camera>(); }
    private Vector3 m_startLocalPosition;
    private Vector3 m_startForward;
    public float FOV = 35f;


    void Start()
    {
        m_startLocalPosition = transform.localPosition;
        m_startForward = transform.parent ? transform.parent.InverseTransformDirection(transform.forward) : transform.forward;
    }

    void Update()
    {
        if (!Camera) return;
        transform.position = VectorUtil.TransformFOV(transform.parent ? transform.parent.TransformPoint(m_startLocalPosition) : m_startLocalPosition, Camera, FOV);
        transform.rotation = Quaternion.LookRotation(VectorUtil.TransformFOV(transform.parent ? transform.parent.TransformPoint(m_startForward) : m_startForward + transform.position, Camera, FOV) - transform.position, Vector3.up);
    }
}