using System.Collections.Generic;
using UnityEngine;

namespace Yavuz.PathFind
{
    public class GridSystem : MonoBehaviour
    {

        public LayerMask unwalkableMask;
        public Vector2 gridWorldSize;

        public float nodeRadius;

        PathNode[,] grid;

        float nodeDiamater;

        int gridSizeX, gridSizeY;
        private static GridSystem instance;
        public static GridSystem Instance { get { return instance; } }


        private void Awake()
        {
            instance = this;
            nodeDiamater = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiamater);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiamater);
            CreateGrid();
        }
        public int MaxSize
        {
            get { return gridSizeX * gridSizeY; }
        }
        bool walkable;
        Vector3 worldPoint;

        public void CreateGrid()
        {
            grid = new PathNode[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiamater + nodeRadius) + Vector3.up * (y * nodeDiamater + nodeRadius);
                    walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);

                    grid[x, y] = new PathNode(walkable, worldPoint, x, y);
                }
            }
        }
        public PathNode NodeFromWorldPoint(Vector3 worldPos)
        {
            float percentX = (worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPos.y + gridWorldSize.y / 2) / gridWorldSize.y;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);
            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return grid[x, y];
        }
        public List<PathNode> GetNeibours(PathNode node)
        {
            List<PathNode> neibours = new List<PathNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neibours.Add(grid[checkX, checkY]);
                    }
                }

            }
            return neibours;
        }
        public void UpdateGrid(Vector3 worldPos, int x, int y)
        {
            Vector3 min = new Vector3(worldPos.x - (x * nodeRadius), worldPos.y - (y * nodeRadius));
            Vector3 max = new Vector3(worldPos.x + (x * nodeRadius), worldPos.y + (y * nodeRadius));
            for (float i = min.x; i < max.x; i += nodeRadius)
            {
                for (float j = min.y; j < max.y; j += nodeRadius)
                {
                    NodeFromWorldPoint(new Vector3(i, j)).isWalkable = false;
                }
            }
        }

     /*   private void OnDrawGizmos()
        {
            if (grid != null)
            {

                foreach (PathNode item in grid)
                {

                    Gizmos.DrawWireCube(item.worldPosition + Vector3.forward * 10, new Vector3(1, 1, .1f) * (nodeDiamater - 0.01f));
                }
            }
        }*/
    }

}