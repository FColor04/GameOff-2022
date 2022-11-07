using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        public int gridRowCount = 3;
        public int gridColumnCount = 3;

        public int rotation; // 0,1,2,3
        public Vector2Int MinCorner { get => Vector2Int.FloorToInt(transform.position._xz() / RoomController.GRID_SIZE); }

        public virtual Vector2Int[] OccupiedCells
        {
            get
            {
                var tiles = new Vector2Int[gridRowCount * gridColumnCount];
                for (int x = 0; x < gridColumnCount; x++)
                    for (int z = 0; z < gridRowCount; z++)
                        tiles[x * gridRowCount + z] = MinCorner + new Vector2Int(x, z);
                return tiles;
            }
        }

        public Vector2Int[] RotatedOccupiedSpaces
        {
            get
            {
                var tiles = OccupiedCells;
                for (int i = 0; i < OccupiedCells.Length; i++)
                    tiles[i] = (tiles[i] - MinCorner).Rotate(rotation) + MinCorner;
                return tiles;
            }

        }

        public List<Doorway> doorways; //custom handle gizmos for doorways? topdown view with clickable tiles? 
    }

    [System.Serializable]
    public struct Doorway{
        public Vector2Int position;
        public Vector2Int direction;
    }
}