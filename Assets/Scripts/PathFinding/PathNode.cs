using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yavuz.PathFind
{
    public class PathNode : IHeapItem<PathNode>
    {
        public bool isWalkable;
        public Vector3 worldPosition;
        public int gridX, gridY;

        public int gCost, hCost;
        int heaapIndex;
        public int FCost { get { return hCost + gCost; } }

        public int HeapIndex { get => heaapIndex; set => heaapIndex = value; }

        public PathNode parent;
        public PathNode(bool isWalkable, Vector3 worldPosition, int _gridX, int _gridY)
        {
            this.isWalkable = isWalkable;
            this.worldPosition = worldPosition;
            this.gridX = _gridX;
            this.gridY = _gridY;


        }

        public int CompareTo(PathNode other)
        {
            int compare = FCost.CompareTo(other.FCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }

}