using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [HideInInspector]
        public RoomController controller;
        [Range(1, 40)]
        public float width = 3;
        [Range(1, 40)]
        public float depth = 3;

        public float X => transform.position.x;
        public float Z => transform.position.z;
        
        [HideInInspector]
        public List<Exit> exits = new();
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
            var offset = new Vector3(Mathf.Floor(width / 2f), -0.001f, Mathf.Floor(depth / 2f));
            Gizmos.DrawCube(transform.position + new Vector3(width / 2f, 0f, depth / 2f) - offset, new Vector3(width, 0.1f, depth));
        }

        private void OnValidate()
        {
            exits = GetComponentsInChildren<Exit>().ToList();
        }

        public bool PropagateExit(Exit exit, List<Room> pool, out Room newInstance)
        {
            var newRoom = Instantiate(pool.Random(), Vector3.zero, Quaternion.identity);
            newRoom.controller = controller;
            
            var propagatedExit = exit;
            var randomExit = newRoom.exits.Random();
            
            //Match exit rotations
            newRoom.transform.eulerAngles = -randomExit.transform.localEulerAngles + propagatedExit.transform.localEulerAngles + Vector3.up * 180f;
            newRoom.transform.SetParent(propagatedExit.transform);
            newRoom.transform.localPosition = newRoom.transform.localRotation * -randomExit.transform.localPosition;

            if (controller.CheckRectCollision(newRoom.X, newRoom.Z, newRoom.width, newRoom.depth))
            {
                Destroy(newRoom.gameObject);
                newInstance = null;
                return false;
            }
            controller.instances.Add(newRoom);
            newInstance = newRoom;
            return true;
        }
        
        public bool PropagateRandomExit(List<Room> pool, out Room newInstance)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (PropagateExit(exits.Random(), pool, out Room roomInstance))
                {
                    newInstance = roomInstance;
                    return true;
                }
            }

            newInstance = null;
            return false;
        }

        public int PropagateEveryExit(List<Room> pool, out List<Room> newRooms)
        {
            newRooms = new();
            int propagatedCount = 0;
            for (var i = 0; i < exits.Count; i++)
            {
                var exit = exits[i];
                if (PropagateExit(exit, pool, out var room))
                {
                    propagatedCount++;
                    newRooms.Add(room);
                }
            }

            return propagatedCount;
        }
    }
}
