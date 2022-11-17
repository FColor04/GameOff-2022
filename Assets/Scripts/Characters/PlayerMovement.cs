using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(PlayerCamera))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerCamera cached_playerCamera;
    private PlayerCamera PlayerCamera { get => cached_playerCamera ??= GetComponent<PlayerCamera>(); }

    private MovementController cached_movementController;
    private MovementController MovementController { get => cached_movementController ??= GetComponent<MovementController>(); }

    public Vector2 MovementInput { get; set; }
    [field: SerializeField] public float Speed { get; set; } = 7f;
    public readonly Dictionary<string, float> MovementSpeedModifiers = new();

    void FixedUpdate()
    {
        MovementController.XZPlaneMovement = Quaternion.Euler(0, 0, -PlayerCamera.RY) * MovementInput * Speed * MovementSpeedModifiers.Values.Aggregate(1f, (x, y) => x * y);
    }
}
