using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private int m_gridColumnCount = 3;
        [SerializeField] private int m_gridRowCount = 3;

        private int cached_rotation = 0;
        public int GridRowCount
        {
            get => m_gridRowCount;
            set
            {
                if (m_gridRowCount == value) return;
                m_gridRowCount = value;
                cached_OccupiedCells = null;
                cached_RotatedOccupiedCells = null;
            }
        }
        public int GridColumnCount
        {
            get => m_gridColumnCount;
            set
            {
                if (m_gridColumnCount == value) return;
                m_gridColumnCount = value;
                cached_OccupiedCells = null;
                cached_RotatedOccupiedCells = null;
            }
        }
        public int Rotation
        {
            get => cached_rotation;
            set
            {
                if (cached_rotation == value) return;
                cached_rotation = value;
                cached_OccupiedCells = new Vector2Int[0];
                cached_RotatedOccupiedCells = new Vector2Int[0];
            }
        } // 0,1,2,3

        public Vector2Int MinCorner { get => Vector2Int.FloorToInt(transform.position._xz() / RoomController.GRID_SIZE); }
        private Vector2Int[] cached_OccupiedCells = null;
        private Vector2Int[] cached_RotatedOccupiedCells = null;

        public virtual Vector2Int[] OccupiedCells
        {
            get
            {
                if (cached_OccupiedCells != null
                    && cached_OccupiedCells.Length == m_gridRowCount * m_gridColumnCount)
                    return cached_OccupiedCells;
                cached_OccupiedCells = new Vector2Int[m_gridRowCount * m_gridColumnCount];
                for (int x = 0; x < m_gridColumnCount; x++)
                    for (int z = 0; z < m_gridRowCount; z++)
                        cached_OccupiedCells[x * m_gridRowCount + z] = new Vector2Int(x, z);
                cached_RotatedOccupiedCells = null;
                return cached_OccupiedCells;
            }
        }

        public Vector2Int[] RotatedOccupiedSpaces
        {
            get
            {
                if (cached_RotatedOccupiedCells != null
                    && cached_RotatedOccupiedCells.Length == m_gridRowCount * m_gridColumnCount
                    && cached_OccupiedCells.Length == m_gridRowCount * m_gridColumnCount)
                    return cached_RotatedOccupiedCells;
                cached_RotatedOccupiedCells = OccupiedCells.ToArray();
                for (int i = 0; i < cached_RotatedOccupiedCells.Length; i++)
                    cached_RotatedOccupiedCells[i] = (cached_RotatedOccupiedCells[i]).Rotate(Rotation);
                return cached_RotatedOccupiedCells;
            }

        }

        public List<Doorway> doorways; //custom handle gizmos for doorways? topdown view with clickable tiles? 
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            foreach (var rotatedTile in RotatedOccupiedSpaces)
                Gizmos.DrawWireCube((((Vector3)(Vector2)(rotatedTile + MinCorner))._x0y() + Vector3.one._x0z() / 2f) * RoomController.GRID_SIZE, Vector3.one._x0z() * RoomController.GRID_SIZE);
            Gizmos.color = Color.magenta;
            foreach (var doorway in doorways)
            {
                var position = (((Vector3)(Vector2)(doorway.position.Rotate(Rotation) + MinCorner))._x0y() + Vector3.one._x0z() / 2f) * RoomController.GRID_SIZE;
                Gizmos.DrawCube(position, Vector3.one._x0z() * RoomController.GRID_SIZE);
                Gizmos.DrawCube(position + ((Vector3)(Vector2)doorway.direction)._x0y() * RoomController.GRID_SIZE, Vector3.one._x0z() * RoomController.GRID_SIZE);
            }            
        }

    }

    [System.Serializable]
    public struct Doorway
    {
        public Vector2Int position;
        public Vector2Int direction;
    }

}