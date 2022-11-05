using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [field: SerializeField] public Transform Camera { get; set; }
    
    
    public float MinAngle = -90f + float.Epsilon, MaxAngle = 90f - float.Epsilon;
    public float smoothingTime = 0.1f;
    public float movementSmoothingTime = 0.1f;
    public float sidewaysTilt = 0.05f;
    public float sidewaysMovement = 0.05f;
    public float maxZrotation = 5f;
    
    public Vector2 TurnSpeed { get; set; }

    private float rx, ry;
    public float RY { get => ry; }
    public float RX { get => rx; }
    private Rigidbody _rigidbody;
    private PlayerMovement _playerMovement;
    private Vector3 velocityTilt;
    private Vector3 velocityMovement;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        rx += TurnSpeed.x;
        ry += TurnSpeed.y;
        rx = Mathf.Clamp(rx, MinAngle, MaxAngle);
        var relativeVelocity = Quaternion.Euler(0, ry, 0) * _rigidbody.velocity;
            
        Camera.localEulerAngles = SmoothDampEuler(Camera.localEulerAngles,
            new Vector3(-rx, ry, Mathf.Clamp(relativeVelocity.x * sidewaysTilt, -maxZrotation, maxZrotation)), ref velocityTilt, smoothingTime);
        Camera.localPosition = Vector3.SmoothDamp(Camera.localPosition, Quaternion.Euler(0, ry, 0) * Vector3.left * (_playerMovement.MovementInput.x * (-relativeVelocity.x * sidewaysMovement)), 
            ref velocityMovement, movementSmoothingTime);
    }
    
    public static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime)
    {
        return new Vector3(
            Mathf.SmoothDampAngle(current.x, target.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(current.y, target.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(current.z, target.z, ref currentVelocity.z, smoothTime)
        );
    }
}
