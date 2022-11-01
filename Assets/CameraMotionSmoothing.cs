using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraMotionSmoothing : MonoBehaviour
{
    public float smoothingDuration;
    public AnimationCurve smoothingInfluenceCurveXZ;
    public AnimationCurve smoothingInfluenceCurveY;
    private Vector3 smoothingPositionBuffer;
    public Transform followTarget;
    public float maxDistance = .5f;

    private List<(Vector3 position, float timestamp)> smoothingBuffer = new();

    public Vector3 SmoothedPosition
    {
        get
        {
            if (smoothingBuffer.Count == 0) return followTarget.position;
            var smoothedPosition = Vector3.zero;
            var totalWeightXZ = float.Epsilon;
            var totalWeightY = float.Epsilon;
            foreach (var entry in smoothingBuffer)
            {
                var position = entry.position;
                var t = 1f - ((Time.time - entry.timestamp) / smoothingDuration);
                var weightY = smoothingInfluenceCurveY.Evaluate(t);
                var weightXZ = smoothingInfluenceCurveXZ.Evaluate(t);
                totalWeightY += weightY;
                totalWeightXZ += weightXZ;
                position.Scale(new(weightXZ, weightY, weightXZ));
                smoothedPosition += position;
            }

            var unclampedSmoothPos = new Vector3(smoothedPosition.x / totalWeightXZ,
                                                 smoothedPosition.y / totalWeightY,
                                                 smoothedPosition.z / totalWeightXZ);
            //clamp to max distance
            var deltaPosition = unclampedSmoothPos - followTarget.position;
            var sMinMagnitude = Math.sMin(deltaPosition.magnitude, maxDistance, 1f);
            deltaPosition = Vector3.ClampMagnitude(deltaPosition, maxDistance);
            return followTarget.position + deltaPosition;
        }
    }

    public void Update()
    {
        smoothingBuffer = smoothingBuffer.Where(entry => Time.time - entry.timestamp < smoothingDuration).ToList();
        smoothingBuffer.Add((followTarget.position, Time.time));
    }

    void LateUpdate()
    {
        transform.position = SmoothedPosition;
    }
}

