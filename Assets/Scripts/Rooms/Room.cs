using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        private RoomController _controller;

        public RoomController controller {
            get => _controller;
            set
            {
                _controller = value;
                foreach (var exit in exits)
                {
                    exit.controller = value;
                }
            }
        }
        [Range(1, 40)]
        public float width = 3;
        [Range(1, 40)]
        public float depth = 3;

        public float X => transform.position.x;
        public float Z => transform.position.z;
        public Bounds Bounds => Mathf.Abs(transform.eulerAngles.y) % 180 > 0.1f ? RoomController.GetRect(X, Z, depth, width) : RoomController.GetRect(X, Z, width, depth);

        [HideInInspector]
        public List<Exit> exits = new();
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
            Gizmos.DrawCube(Bounds.center, Bounds.size);
        }

        private void OnValidate()
        {
            exits = GetComponentsInChildren<Exit>().ToList();
        }
    }
}
