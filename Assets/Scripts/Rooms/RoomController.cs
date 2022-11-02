using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomController : MonoBehaviour
    {
        public List<Room> pool = new ();
        public List<Room> endpoints = new();
        public List<Room> instances = new ();
        public int roomCount;
        
        private void Awake()
        {
            instances.Clear();
            
            var root = Instantiate(pool.Random(), transform);
            root.controller = this;
            instances.Add(root);
            root.PropagateEveryExit(pool, out _);
            var lastRoom = instances.Random();
            for (int i = 0; i < roomCount; i++)
            {
                lastRoom.PropagateRandomExit(pool, out var newRoom);
                if (newRoom != null)
                {
                    lastRoom = newRoom;
                }
                else
                {
                    lastRoom = instances.Random();
                }
            }

            for (var i = 0; i < instances.Count; i++)
            {
                instances[i].PropagateEveryExit(endpoints, out _);
            }
        }

        public bool CheckRectCollision(float x, float z, float width, float depth)
        {
            foreach (var room in instances)
            {
                if (room.X < x + width &&
                    room.X + room.width > x &&
                    room.Z < z + depth &&
                    room.Z + room.depth > z)
                    return true;
            }

            return false;
        }
    }
}
