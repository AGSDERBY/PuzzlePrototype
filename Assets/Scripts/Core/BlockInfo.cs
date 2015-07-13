using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Puzzle.Block.Core
{
    [Serializable]
    public class BlockInfo
    {
        public bool isChecked = false;
        public float reSpawnDelay;
        private float reSpawnTime;
        public float ReSpawnTime
        {
            get
            {
                return reSpawnTime;
            }
        }

        public Vector3 Position { get; set; }
        public Coordinate Coordinate { get; internal set; }
        public BoardController Controller { get; internal set; }

        public void SetReSpawnTime(float currentTime)
        {
            reSpawnTime = currentTime + reSpawnDelay;
        }

        public bool IsReSpawnable(float currentTime)
        {
            if(reSpawnTime < currentTime)
            {
                return true;
            }

            return false;
        }
    }

    [Serializable]
    public class Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }
    }
}


