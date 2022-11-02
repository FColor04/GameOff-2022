using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityController : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 CurrentVelocity { get => rb.velocity; }

    public struct MovementOverride
    {
        public enum BlendMode { Additive, Overwrite, Maximum }
        public BlendMode blendMode;
        public Func<float, float> speedCurve;
        public Vector3 direction;

        public MovementOverride(Vector3 direction,
                                Func<float, float> speedCurve,
                                BlendMode blendMode = BlendMode.Maximum)
        {
            this.direction = direction;
            this.blendMode = blendMode;
            this.speedCurve = speedCurve;
        }
        public MovementOverride(Vector3 direction,
                                float constantSpeed,
                                BlendMode blendMode = BlendMode.Maximum)
        : this(direction, (t) => constantSpeed, blendMode) { }

        public MovementOverride(Vector3 desiredVelocity,
                                BlendMode blendMode = BlendMode.Maximum
                                ) : this(desiredVelocity.normalized, (t) => desiredVelocity.magnitude, blendMode) { }        
    }

    private struct MovementOverrideInstance
    {
        public MovementOverride movementOverride;
        public float startTime;
        public float duration;
        public MovementOverrideInstance(MovementOverride movementOverride, float startTime, float duration)
        {
            this.movementOverride = movementOverride;
            this.startTime = startTime;
            this.duration = duration;
        }
    }
    private Dictionary<int, List<MovementOverrideInstance>> movementOverrideInstances = new Dictionary<int, List<MovementOverrideInstance>>();

    void Start()
    {
        // Grab self's Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    public void Clear() => movementOverrideInstances.Clear();

    public void AddOverwriteMovement(MovementOverride movementOverride, float duration, int priority = 0)
    {
        (   //ensure list
            movementOverrideInstances.ContainsKey(priority) ?
            movementOverrideInstances[priority] :
            movementOverrideInstances[priority] = new List<MovementOverrideInstance>()
        ).Add(new MovementOverrideInstance(movementOverride, Time.time, duration));
    }
    private void FixedUpdate()
    {
        //apply movement overrides onto current movement
        var desiredVelocity = CurrentVelocity;
        ProcessMovementOverrides(ref desiredVelocity);

        //apply such an impulse to the rb, that it reaches the desired velocity
        rb.AddForce((desiredVelocity - rb.velocity), ForceMode.VelocityChange);

        //remove expired entries (after applying their effects at least once)
        RemoveExpiredMovementOverrides();
    }




    //proplems:
    //how should I handle multiple overwrite movements with the same priority?    
    private void ProcessMovementOverrides(ref Vector3 currentVelocity)
    {
        var startVelocity = currentVelocity; //memorize start movement in case a movementOverride has its blend mode set to overwrite
        //iterate over remaining entries
        foreach (var priority in movementOverrideInstances.Keys.OrderByDescending(x => x))
            foreach (var movementOverrideInstance in movementOverrideInstances[priority])
            {
                var t = Mathf.Clamp01((Time.time - movementOverrideInstance.startTime) / movementOverrideInstance.duration);
                var movementOverrideData = movementOverrideInstance.movementOverride;

                var speed = movementOverrideData.speedCurve(t);
                var newMovement = speed * movementOverrideData.direction.normalized;

                switch (movementOverrideData.blendMode)
                {
                    case MovementOverride.BlendMode.Additive:
                        currentVelocity += newMovement;
                        break;
                    case MovementOverride.BlendMode.Maximum: //pick whatever movement override has a larger magnitude
                        currentVelocity = (currentVelocity.sqrMagnitude > speed * speed) ? currentVelocity : newMovement;
                        break;
                    case MovementOverride.BlendMode.Overwrite: //overwrite all lower priority movement for this physics step
                        currentVelocity += newMovement - startVelocity;
                        return; // we can exit early, because we sorted by priority
                }
            }
    }

    private void RemoveExpiredMovementOverrides()
    {
        //call ToList() to copy the keys to avoid collection modified exception
        foreach (var priority in movementOverrideInstances.Keys.ToList())
        {
            //filter out expired entries by checking normalized time
            var expired = movementOverrideInstances[priority].Where((movementOverrideInstance) =>
                    (Time.time - movementOverrideInstance.startTime) / movementOverrideInstance.duration >= 1f).ToList();
            movementOverrideInstances[priority] = movementOverrideInstances[priority].Except(expired).ToList();
        }
    }
}
