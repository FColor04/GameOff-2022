using System;
using UnityEngine;

namespace Rooms
{
    public class Exit : MonoBehaviour
    {
        [HideInInspector]
        public RoomController controller;
        public bool occupied;
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.3f);
            Gizmos.DrawSphere(transform.position, 0.15f);
            Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.15f);
            Gizmos.DrawSphere(transform.position + Vector3.up, 0.15f);
            Gizmos.color = new Color(0f, 0f, 1f, 0.6f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position + transform.up / 4f + transform.forward / 1.4f, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position - transform.up / 4f + transform.forward / 1.4f, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position + transform.right / 4f + transform.forward / 1.4f, transform.position + transform.forward);
            Gizmos.DrawLine(transform.position - transform.right / 4f + transform.forward / 1.4f, transform.position + transform.forward);
        }
    }
}