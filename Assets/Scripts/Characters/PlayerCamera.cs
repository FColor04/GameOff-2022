using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; set; }

    public float MinAngle = -90f + float.Epsilon, MaxAngle = 90f - float.Epsilon;

    public Vector2 TurnSpeed { get; set; }

    private float rx, ry;
    public float RY { get => ry; }
    public float RX { get => rx; }

    void Update()
    {
        rx += TurnSpeed.x;
        ry += TurnSpeed.y;
        rx = Mathf.Clamp(rx, MinAngle, MaxAngle);
        Camera.transform.localRotation = Quaternion.Euler(-rx, ry, Camera.transform.eulerAngles.z);
    }
}
