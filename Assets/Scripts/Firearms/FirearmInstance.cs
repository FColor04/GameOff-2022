using System.Collections;
using System.Collections.Generic;
using InteractionSystem;
using UnityEngine;
using UnityEngine.VFX;

namespace Firearms
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class FirearmInstance : MonoBehaviour, IInteractable
    {
        public string Message => $"Pick up {FirearmData?.name}";

        [Header("FirearmModel References")]
        [SerializeReference] private GameObject Model;
        [SerializeReference] private Transform muzzlePositionTransform;
        public Transform MuzzlePositionTransform { get => muzzlePositionTransform; }
        [SerializeField] private FirearmData firearmData;
        public FirearmData FirearmData { get => firearmData; }
        [SerializeField] private Animator animator;
        [SerializeReference] private VisualEffect droppedStateVfx;
        private bool equipped;
        private Collider m_collider;
        private Collider Collider { get => m_collider ??= GetComponent<Collider>(); }
        private Rigidbody m_rigidbody;
        private Rigidbody Rigidbody { get => m_rigidbody ??= GetComponent<Rigidbody>(); }

        public Vector2 PlayerLookDeltaAngle { get; set; }
        public Vector3 PlayerLocalMoveVelocity { get; set; }


        public void Execute(PlayerInteraction sender)
        {
            var equipment = sender.GetComponent<PlayerEquipment>();
            var prevFirearm = equipment.EquippedFirearm;
            equipment.EquippedFirearm = this;
            prevFirearm?.OnDrop(sender.playerCamera.transform.forward * 15f);
            this.OnPickup();
        }

        public void OnEnable() { if (Model) Model.transform.localPosition = -(Model.VisualBounds().center - Model.transform.position); }

        public void OnPickup()
        {
            Model.transform.localPosition = Vector3.zero;
            Collider.enabled = false;
            Rigidbody.isKinematic = true;
            Rigidbody.interpolation = RigidbodyInterpolation.None;
            Rigidbody.detectCollisions = false;
            transform.localRotation = Quaternion.identity;
            droppedStateVfx.enabled = false;
            equipped = true;
        }

        public void OnDrop(Vector3 force)
        {
            Model.transform.localPosition = -(Model.VisualBounds().center - Model.transform.position);
            Collider.enabled = true;
            Rigidbody.isKinematic = false;
            Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            Rigidbody.detectCollisions = true;
            Rigidbody.velocity = force;
            droppedStateVfx.enabled = true;
            equipped = false;
        }


        #region 
        public RuntimeAnimatorController AnimationSet { get => animator.runtimeAnimatorController; set => animator.runtimeAnimatorController = value; }
        public float FallSpeed { set => animator.SetFloat("FallSpeed", value); }
        private Vector2 LookVelocity
        {
            set
            {
                animator.SetFloat("DeltaAngleX", value.x);
                animator.SetFloat("DeltaAngleY", value.y);
            }
            get => new(animator.GetFloat("DeltaAngleX"), animator.GetFloat("DeltaAngleY"));
        }
        void FixedUpdate()
        {
            LookVelocity = Vector2.Lerp(PlayerLookDeltaAngle + PlayerLocalMoveVelocity._y0() + PlayerLocalMoveVelocity._0x() * .75f, LookVelocity, 0.5f);
        }

        void LateUpdate()
        {
            if (!equipped) return; animator.Update(Time.deltaTime);
        }

        public void PlayRecoil(float strength = 1f)
        {
            if (!equipped) return;
            animator.SetFloat("Recoil Strength", strength);
            animator.SetTrigger("Recoil");
        }
        #endregion
        public void NotifyLookedAt()
        {

        }

        public void NotifyLookedAway()
        {

        }
    }
}