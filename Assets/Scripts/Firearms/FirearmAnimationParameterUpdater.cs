using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firearms
{
    public class FirearmAnimationParameterUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerEquipment pe;
        [SerializeField] private PlayerCamera pc;
        [SerializeField] private VelocityController vc;

        void FixedUpdate()
        {
            if (pe?.EquippedFirearm == null) return;
            pe.EquippedFirearm.PlayerLookDeltaAngle = pc?.TurnSpeed ?? Vector2.zero;
            pe.EquippedFirearm.PlayerLocalMoveVelocity = Quaternion.Euler(0, -pc.RY, 0) * vc?.CurrentVelocity ?? Vector3.zero;
        }
    }
}