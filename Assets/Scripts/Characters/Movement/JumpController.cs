using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(VelocityController))]
public class JumpController : MonoBehaviour
{
    private MovementController cached_movementController;
    public MovementController MovementController { get => cached_movementController ??= GetComponent<MovementController>(); }

    private VelocityController cached_velocityController;
    public VelocityController VelocityController { get => cached_velocityController ??= GetComponent<VelocityController>(); }

    public List<JumpInfo> jumps = new() {
        new(2f),
    };
    private Queue<JumpInfo> remainingJumps = new();

    public int RemainingJumps { get => remainingJumps.Count; }

    public bool JumpInput { get; set; }
    private bool lastJumpInput;

    public void FixedUpdate()
    {
        bool grounded = MovementController.Grounded; //get grounded
        if (grounded && remainingJumps.Count < jumps.Count) remainingJumps = new(jumps); //refresh jumps if grounded        
        var jumpPressedThisPhysicsUpdate = !lastJumpInput && JumpInput; //determine if jump was freshly pressed
        lastJumpInput = JumpInput;

        var shouldPerformJump = (grounded && JumpInput) || jumpPressedThisPhysicsUpdate;
        if (!shouldPerformJump || remainingJumps.Count == 0) return;
        var jump = remainingJumps.Dequeue();
        VelocityController.AddOverwriteMovement(new VelocityController.MovementOverride(jump.direction, jump.CalculatedVelocity, VelocityBlendMode.Overwrite), 0f, 0);
    }

    [System.Serializable]
    public struct JumpInfo
    {
        public float desiredHeight;
        public Vector3 direction;

        public JumpInfo(float desiredHeight) : this(desiredHeight, Vector3.up) { }
        public JumpInfo(float desiredHeight, Vector3 direction)
        {
            this.desiredHeight = desiredHeight;
            this.direction = direction;
        }

        public float CalculatedVelocity
        {
            get
            {
                var gy = Mathf.Abs(Physics.gravity.y);
                var vy = Mathf.Sqrt(gy * 2f * desiredHeight);
                var vmag = vy / direction.y;
                return vmag;
            }
        }
    }
}
