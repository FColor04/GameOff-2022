using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class RoomController : MonoBehaviour
    {
        //Move to SO
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

            int limit = 0;
            var roomQueue = GetScrambledQueue(pool);
            for (int i = 0; i < roomCount + limit; i++)
            {
                if (roomQueue.TryDequeue(out Room newRoom))
                {
                    if (!PropagateExit(instances.Where(room => room.exits.Any(exit => !exit.occupied)).ToList()
                            .Random().exits.Where(exit => !exit.occupied).ToList().Random(), newRoom, out _))
                        limit++;
                }
                else
                {
                    roomQueue = GetScrambledQueue(pool);
                    i--;
                }

                if (limit > 100)
                {
                    Debug.LogWarning("Limit reached, breaking");
                    break;
                }
            }
        }

        public static Bounds GetRect(float x, float z, float width, float depth)
        {
            return new Bounds(){center = new Vector3(x, 0, z), size = new Vector3(width, 0.1f, depth)};
        }

        public static Queue<T> GetScrambledQueue<T>(List<T> list)
        {
            var scrambledList = list.ToList();
            var queueA = new Queue<T>();
            while (scrambledList.Count > 0)
            {
                int i = Random.Range(0, scrambledList.Count);
                queueA.Enqueue(scrambledList[i]);
                scrambledList.RemoveAt(i);
            }

            return queueA;
        }

        public bool PropagateExit(Exit exit, Room room, out Room newInstance)
        {
            //Check for collisions here instead of deeper... To be done soon!
            
            var newRoom = Instantiate(room, Vector3.zero, Quaternion.identity);
            newRoom.controller = this;
            
            var propagatedExit = exit;
            var randomExit = newRoom.exits.Random();
            
            //Match exit rotations
            float yRot = randomExit.transform.eulerAngles.y + propagatedExit.transform.eulerAngles.y + 180;
            newRoom.transform.eulerAngles = new Vector3(0, yRot, 0);
            newRoom.transform.position = propagatedExit.transform.position + (newRoom.transform.rotation * randomExit.transform.localPosition);
            newRoom.transform.SetParent(propagatedExit.transform, true);

            
            if (instances.Any(instance => instance.Overlaps(newRoom)))
            {
                Destroy(newRoom.gameObject, 3);
                newInstance = null;
                return false;
            }

            randomExit.occupied = true;
            propagatedExit.occupied = true;
            
            instances.Add(newRoom);
            newInstance = newRoom;
            return true;
        }
    }
    
    public static partial class Utilities {
        public static bool Overlaps(this Room roomA, Room roomB)
        {
            return roomA.Bounds.Intersects(roomB.Bounds);
        }
    }
}
