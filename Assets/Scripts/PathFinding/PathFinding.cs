using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yavuz.PathFind
{
    public class PathFinding : MonoBehaviour
    {
        PathRequestManager pathRequestManager;
        GridSystem gridSystem;
        private void Awake()
        {
            gridSystem = GetComponent<GridSystem>();
            pathRequestManager = GetComponent<PathRequestManager>();
        }

        IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            PathNode startNode = gridSystem.NodeFromWorldPoint(startPos);
            PathNode targetNode = gridSystem.NodeFromWorldPoint(targetPos);
            if (targetNode.isWalkable)
            {


                Heap<PathNode> openList = new Heap<PathNode>(gridSystem.MaxSize);
                HashSet<PathNode> cloasedLised = new HashSet<PathNode>();
                openList.Add(startNode);

                while (openList.Count > 0)
                {
                    PathNode currentNode = openList.RemoveFirst();

                    cloasedLised.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        openList.Add(currentNode);
                        pathSuccess = true;

                        break;
                    }
                    foreach (PathNode item in gridSystem.GetNeibours(currentNode))
                    {
                        if (!item.isWalkable || cloasedLised.Contains(item))
                        {
                            continue;
                        }
                        int neibourCost = currentNode.gCost + GetDistance(currentNode, item);
                        if (neibourCost < item.gCost || !openList.Contains(item))
                        {
                            item.gCost = neibourCost;
                            item.hCost = GetDistance(item, targetNode);
                            item.parent = currentNode;
                            if (!openList.Contains(item))
                            {
                                openList.Add(item);
                            }
                            else
                                openList.UpdateItem(item);
                        }
                    }
                }
            }
            yield return null;
            if (pathSuccess)
            {
                wayPoints = RetracePath(startNode, targetNode);
            }
            pathRequestManager.FinishedProcessingPath(wayPoints, pathSuccess);
        }

        public void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
        {
            StartCoroutine(FindPath(pathStart, pathEnd));
        }

        Vector3[] RetracePath(PathNode startNode, PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            PathNode current = endNode;

            while (current != startNode)
            {
                path.Add(current);
                current = current.parent;
            }
            if (path.Count > 0)
                path.Insert(0, endNode);
            Vector3[] wayPoints = SimplifyPath(path);
            Array.Reverse(wayPoints);

            return wayPoints;
        }
        Vector3[] SimplifyPath(List<PathNode> path)
        {
            List<Vector3> wayPoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (directionNew != directionOld)
                {
                    wayPoints.Add(path[i].worldPosition);
                }
                directionOld = directionNew;
            }
            if (path.Count > 0)
                wayPoints.Insert(0, path[0].worldPosition); // burdan devam
            return wayPoints.ToArray();
        }
        int GetDistance(PathNode nodeA, PathNode nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

            if (distanceX < distanceY)
                return 14 * distanceY + 10 * (distanceX - distanceY);
            return
                14 * distanceX + 10 * (distanceY - distanceX);
        }

    }

}