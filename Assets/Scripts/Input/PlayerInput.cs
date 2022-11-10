using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private GameplayControls controls;
    [SerializeField] private PlayerMovement pm;
    [SerializeField] private PlayerCamera pc;
    [SerializeField] private PlayerInteraction pi;
    [SerializeField] private CrouchController cc;
    [SerializeField] private JumpController jc;

    void Awake()
    {
        controls = new();
        controls.Gameplay.Move.performed += ReadMoveInput;
        controls.Gameplay.Look.performed += ReadLookInput;
        controls.Gameplay.Crouch.started += ReadCrouchInput;
        controls.Gameplay.Jump.started += ReadJumpInput;

        controls.Gameplay.Move.canceled += ReadMoveInput;
        controls.Gameplay.Look.canceled += ReadLookInput;
        controls.Gameplay.Crouch.canceled += ReadCrouchInput;
        controls.Gameplay.Jump.canceled += ReadJumpInput;
        controls.Gameplay.Interact.canceled += ReadInteractionInput;
    }
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controls.Gameplay.Move.Enable();
        controls.Gameplay.Look.Enable();
        controls.Gameplay.Jump.Enable();
        controls.Gameplay.Crouch.Enable();
        controls.Gameplay.Interact.Enable();
    }

    void Disable()
    {
        Cursor.lockState = CursorLockMode.None;
        controls.Gameplay.Move.Disable();
        controls.Gameplay.Look.Disable();
        controls.Gameplay.Jump.Disable();
        controls.Gameplay.Crouch.Disable();
        controls.Gameplay.Interact.Disable();
    }
    void OnDestroy() => controls.Dispose();

    void ReadMoveInput(InputAction.CallbackContext callbackContext) => pm.MovementInput = callbackContext.ReadValue<Vector2>();
    void ReadLookInput(InputAction.CallbackContext callbackContext) => pc.TurnSpeed = callbackContext.ReadValue<Vector2>()._yx();
    void ReadCrouchInput(InputAction.CallbackContext callbackContext) => cc.Crouching = (callbackContext.phase == InputActionPhase.Started || callbackContext.phase == InputActionPhase.Performed);
    void ReadJumpInput(InputAction.CallbackContext callbackContext) => jc.JumpInput = (callbackContext.phase == InputActionPhase.Started || callbackContext.phase == InputActionPhase.Performed);
    void ReadInteractionInput(InputAction.CallbackContext callbackContext) => pi.Interact();
}
