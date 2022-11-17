using System;
using UnityEngine;
namespace Firearms
{
    [CreateAssetMenu]
    public class CrosshairData : ScriptableObject
    {
        public enum InsideShape
        {
            Nothing,
            Dot,
            Cross,
            NGon
        }
        public enum OutsideShape
        {
            Nothing,
            Lines,
            HLines,
            VLines,
            NGon,
        }
        public enum CrosshairBehaviour
        {
            Nothing,
            Offset,
            InverseOffset,
            Breathe,
            Spin
        }

        [Header("Inside")]
        public InsideShape insideShape;
        [Range(3, 30)]
        public int insideNgon = 3;
        public Color insideColor = Color.white;
        [HideInInspector]
        public float runtimeInsideOffset;
        public float insideOffset;
        public float insideThickness;
        [HideInInspector]
        public float runtimeInsideRotation;
        public float insideRotation;

        [Header("Outside")]
        public OutsideShape outsideShape;
        [Range(3, 30)]
        public int outsideNgon = 3;
        public Color outsideColor = Color.white;
        [HideInInspector]
        public float runtimeOutsideOffset;
        public float outsideOffset;
        [HideInInspector]
        public float runtimeOutsideRotation;
        public float outsideRotation;
        public float outsideThickness;
        public float outsideLength;

        public CrosshairBehaviour insideBehaviour;
        public CrosshairBehaviour outsideBehaviour;
        public float outsideBehaviourAmount;
        public float outsideBehaviourStrength;
        [HideInInspector]
        public float runtimeOutsideBehaviourAmount;
        public float outsideBehaviourDecay;

        public void ResetRuntimeValues()
        {
            runtimeInsideOffset = insideOffset;
            runtimeInsideRotation = insideRotation;

            runtimeOutsideOffset = outsideOffset;
            runtimeOutsideRotation = outsideRotation;

            runtimeOutsideBehaviourAmount = outsideBehaviourAmount;
        }

        private void OnValidate()
        {
            ResetRuntimeValues();
            /* foreach (var crosshair in FindObjectsOfType<CrosshairRenderer>())
            {
                crosshair.SetVerticesDirty();
            } */
        }
    }
}