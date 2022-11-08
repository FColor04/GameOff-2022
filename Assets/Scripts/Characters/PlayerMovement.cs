using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(PlayerCamera))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerCamera cached_playerCamera;
    private PlayerCamera PlayerCamera { get => cached_playerCamera ??= GetComponent<PlayerCamera>(); }

    private MovementController cached_movementController;
    private MovementController MovementController { get => cached_movementController ??= GetComponent<MovementController>(); }
    
    private PlayerEquipment cached_playerEquipment;
    private PlayerEquipment PlayerEquipment { get => cached_playerEquipment ??= GetComponent<PlayerEquipment>(); }
    
    public Vector2 MovementInput { get; set; }
    [field: SerializeField] public float Speed { get; set; } = 7f;
    [field: SerializeField] public float DownsightsSpeed { get; set; } = 5f;

    void FixedUpdate()
    {
        MovementController.XZPlaneMovement = Quaternion.Euler(0, 0, -PlayerCamera.RY) * MovementInput * (PlayerEquipment.aimingDownsights ? DownsightsSpeed : Speed);
    }
}
