using UnityEngine;

namespace Characters
{
    public class TargetDummy : MonoBehaviour, IHasHealth
    {
        [SerializeField]
        private Health health = new Health(100);
        public Health Health
        {
            get => health;
            set => health = value;
        }

        [SerializeField]
        private Collider criticalHitBox;
        public Collider CriticalHitBox => criticalHitBox;
        [SerializeField]
        private Collider regularHitBox;
        public Collider RegularHitBox => regularHitBox;

        public void OnHit(float damage)
        {
            Debug.Log("Hit");
        }
        
        public void OnCriticalHit(float damage)
        {
            Debug.Log("Critical Hit");
        }
    }
}