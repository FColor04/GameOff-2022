using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class RoomController : MonoBehaviour
    {
        public const float GRID_SIZE = 1f;
        public Room startRoom;
        public List<Room> pool = new();
        public List<Room> instances = new();
        public int roomCount;

        private void Awake()
        {
            var availableExits = new List<(Vector2Int worldPosition, Vector2Int direction)>();
            var roomCount = 0;
            var exitPointer = 0;
            var lastExitPointer = 0; //prevent infinite loops
            instances.Add(SpawnRoom(startRoom, 0, Vector2Int.zero, null, ref availableExits, ref exitPointer));
            while (roomCount < this.roomCount && availableExits.Count > 0)
            {
                if (exitPointer > lastExitPointer + availableExits.Count * 2)
                    break;
                exitPointer++;
                var exit = availableExits[(exitPointer++) % availableExits.Count]; //round robbin randomness
                var roomPoolPermutation = pool.RandomPermutation();
                foreach (var room in roomPoolPermutation)
                {
                    var hasSpawned = false;
                    var randomDoorwayPermutation = room.doorways.RandomPermutation();
                    foreach (var doorway in randomDoorwayPermutation)
                    {
                        var rotation = RotateMatchExits(exit.direction, -doorway.direction);
                        var rotatedDoorwayPosition = (doorway.position - room.MinCorner).Rotate(rotation) + room.MinCorner;
                        var delta = exit.worldPosition + exit.direction + (room.MinCorner - rotatedDoorwayPosition);
                        var rotatedTiles = room.OccupiedCells.Select(vector => (vector - room.MinCorner).Rotate(rotation) + room.MinCorner + delta).ToArray();
                        if (CheckHasSpace(rotatedTiles))
                        {
                            instances.Add(SpawnRoom(room, rotation, delta, doorway, ref availableExits, ref exitPointer));
                            roomCount++;
                            lastExitPointer = exitPointer;
                            hasSpawned = true;
                            break;
                        }
                    }
                    if (hasSpawned) break;
                }
            }
        }

        private Room SpawnRoom(Room room, int rotation, Vector2Int position, Doorway? usedDoorway, ref List<(Vector2Int worldPosition, Vector2Int direction)> availableExits, ref int exitPointer)
        {
            room = Instantiate<Room>(room);
            room.rotation = rotation;
            room.transform.position = ((Vector3)(Vector2)position)._x0y() * GRID_SIZE + (Vector3.one * GRID_SIZE / 2f)._x0z();
            room.transform.rotation = Quaternion.Euler(0, 90 * rotation, 0);
            foreach (var doorway in room.doorways)
            {
                if (usedDoorway != null && usedDoorway.Value.position == doorway.position && usedDoorway.Value.direction == doorway.direction) continue;
                var doorwayPosition = doorway.position.Rotate(rotation) + room.MinCorner;
                var doorwayDirection = doorway.direction.Rotate(rotation);
                var insertIndex = Random.Range(0, availableExits.Count);
                if (insertIndex <= exitPointer) exitPointer++;
                availableExits.Insert(insertIndex, (doorwayPosition, doorwayDirection)); //insert new doorways at random positions
            }
            return room;
        }

        public int RotateMatchExits(Vector2Int A, Vector2Int B)
        {
            var r1 = A.Rotation();
            var r2 = B.Rotation();
            var r = (r2 - r1 + 4) % 4;
            return r;
        }

        public bool CheckHasSpace(IEnumerable<Vector2Int> positions)
        {
            foreach (var room in instances)
            {
                if (room.RotatedOccupiedSpaces.Intersect(positions).Any())
                    return false;
            }
            return true;
        }
    }
}
