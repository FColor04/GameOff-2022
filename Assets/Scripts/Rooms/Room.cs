using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [Range(3, 40)]
        public int width = 3;
        [Range(3, 40)]
        public int depth = 3;
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

        public void PropagateRandomExit(List<Room> pool)
        {
            //This does not work but is close
            // var propagatedExit = exits.Random();
            // var propagatedExitRotation = propagatedExit.transform.rotation;
            // var propagatedExitPosition = propagatedExit.transform.position - transform.position;
            // var newRoom = Instantiate(pool.Random(), Vector3.zero, Quaternion.identity);
            // newRoom.transform.SetParent(propagatedExit.transform);
            // var randomExit = newRoom.exits.Random();
            // var exitOffset = -randomExit.transform.position + propagatedExitPosition;
            // var exitRotation = Quaternion.Inverse(randomExit.transform.rotation);
            // newRoom.transform.position = transform.position + exitRotation * exitOffset;
            // newRoom.transform.rotation = exitRotation;
        }
    }
}
