using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomController : MonoBehaviour
    {
        public List<Room> pool = new ();

        private void Awake()
        {
            Instantiate(pool.Random(), transform).PropagateRandomExit(pool);
        }
    }
}
