using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MapGeneration.Rooms
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
            var exitPointer = 0;
            SpawnRoom(startRoom, 0, Vector2Int.zero, null, ref availableExits, ref exitPointer);
            while (instances.Count < this.roomCount && availableExits.Count > 0)
            {
                exitPointer++;
                var hasSpawned = false;
                var roomPoolPermutation = pool.RandomPermutation();
                var exitCount = availableExits.Count;
                for (int i = 0; i < exitCount; i++)
                {
                    var exit = availableExits[(exitPointer++) % availableExits.Count]; //round robbin randomness
                    foreach (var roomTemplate in roomPoolPermutation)
                    {
                        var randomDoorwayPermutation = roomTemplate.doorways.RandomPermutation();
                        foreach (var doorway in randomDoorwayPermutation)
                        {
                            var rotation = RotateMatchExits(exit.direction, -doorway.direction);
                            var rotatedDoorwayPosition = (doorway.position - roomTemplate.MinCorner).Rotate(rotation) + roomTemplate.MinCorner;
                            var newMinCornerWorldPosition = exit.worldPosition + exit.direction + (roomTemplate.MinCorner - rotatedDoorwayPosition);
                            var requiredWorldPositions = roomTemplate.OccupiedCells.Select(vector => vector.Rotate(rotation) + newMinCornerWorldPosition).ToArray();
                            if (requiredWorldPositions.Length < roomTemplate.GridRowCount * roomTemplate.GridColumnCount) Debug.LogError("wtf");
                            if (CheckHasSpace(requiredWorldPositions))
                            {
                                availableExits.RemoveAt(exitPointer % availableExits.Count);
                                var roomInstance = SpawnRoom(roomTemplate, rotation, newMinCornerWorldPosition, doorway, ref availableExits, ref exitPointer);
                                hasSpawned = true;
                                break;
                            }
                        }
                        if (hasSpawned) break;
                    }
                    if (hasSpawned) break;
                }
                if (!hasSpawned) break;
            }
            Debug.Log($"spawned {instances.Count} / {this.roomCount} rooms");
        }

        private Room SpawnRoom(Room room, int rotation, Vector2Int position, Doorway? usedDoorway, ref List<(Vector2Int worldPosition, Vector2Int direction)> availableExits, ref int exitPointer)
        {
            room = Instantiate<Room>(room);
            instances.Add(room);
            room.Rotation = rotation;
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

        public bool CheckHasSpace(IEnumerable<Vector2Int> worldSpacePositions)
        {
            foreach (var room in instances)
            {
                var worldSpaceOccupiedTiles = room.RotatedOccupiedSpaces.Select(position => position + room.MinCorner).ToArray();
                foreach (var desiredTile in worldSpacePositions)
                    foreach (var occupiedTile in worldSpaceOccupiedTiles)
                        if (desiredTile == occupiedTile)
                            return false;
            }
            return true;
        }
    }
}
