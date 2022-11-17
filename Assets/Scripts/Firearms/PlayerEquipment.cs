using UnityEngine;
using Firearms;

namespace Firearms
{
    public class PlayerEquipment : MonoBehaviour
    {
        [SerializeField] private Transform FirearmAnchor;
        private FirearmInstance m_equippedFirearm;
        public FirearmInstance EquippedFirearm
        {
            get => m_equippedFirearm; set
            {
                if (m_equippedFirearm == value) return;
                if (m_equippedFirearm)
                {
                    m_equippedFirearm.transform.SetParent(null);
                    m_equippedFirearm.transform.position = transform.position + Vector3.up * 1.5f;
                    m_equippedFirearm.transform.localRotation = Quaternion.identity;
                    m_equippedFirearm.transform.localScale = Vector3.one;
                    m_equippedFirearm.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Firearms"));
                }
                m_equippedFirearm = value;
                if (!m_equippedFirearm) return;
                m_equippedFirearm.transform.SetParent(FirearmAnchor);
                m_equippedFirearm.transform.localPosition = Vector3.zero;
                m_equippedFirearm.transform.localRotation = Quaternion.identity;
                m_equippedFirearm.transform.localScale = Vector3.one;
                m_equippedFirearm.gameObject.SetLayerRecursive(LayerMask.NameToLayer("Player"));
            }
        }

        void LateUpdate()
        {
            if (!EquippedFirearm) return;

        }
    }
}