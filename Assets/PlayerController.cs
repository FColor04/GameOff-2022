using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VelocityController vc;

    private Vector2 accumulatedInput;
    private bool jumpInput;


    void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(KeyCode.Space)) jumpInput = true;
        accumulatedInput += new Vector2(h, v) * Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (accumulatedInput.sqrMagnitude > float.Epsilon)
            vc.AddOverwriteMovement(new(new(accumulatedInput.x, 0, accumulatedInput.y), 3f, VelocityController.MovementOverride.BlendMode.Additive), 0f, 1);
        if (jumpInput)
            vc.AddOverwriteMovement(new(Vector3.up, 10f, VelocityController.MovementOverride.BlendMode.Maximum), 0f, 1);
        vc.AddOverwriteMovement(new(Vector3.up, -.5f, VelocityController.MovementOverride.BlendMode.Additive), 0f, 0);
        vc.AddOverwriteMovement(new(Vector3.up, vc.CurrentVelocity.y, VelocityController.MovementOverride.BlendMode.Overwrite), 0f, -1);
        accumulatedInput = Vector2.zero;
        jumpInput = false;
    }
}
