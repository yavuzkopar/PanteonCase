using System.Collections;
using UnityEngine;
using Yavuz.PathFind;
using Yavuz.Control;

namespace Yavuz.Unite
{
    public class Unit : MonoBehaviour
    {

        [SerializeField] float speed;
        Vector3[] path;
        int targetIndex;

        GridSystem gridSystem;
        public bool isSelected;
        public GameObject sprite;
        private void Start()
        {
            gridSystem = GameObject.FindGameObjectWithTag("Manager").GetComponent<GridSystem>();
            //  PathRequestManager.RequestPath(transform.position, m_Transform.position, OnPathFound);

        }
        private void Update()
        {
            if (SelectionController.Instance.selectedObject == transform)
            {
                sprite.SetActive(true);
                if (Input.GetMouseButtonDown(1))
                {
                    Vector3 targetPos = gridSystem.NodeFromWorldPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).worldPosition;
                    MoveTo(targetPos);
                }

            }
            else
                sprite.SetActive(false);

        }

        public void MoveTo(Vector3 targetPos)
        {
            PathRequestManager.RequestPath(transform.position, targetPos, OnPathFound);
        }

        void OnPathFound(Vector3[] newPath, bool success)
        {
            if (success)
            {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }
        IEnumerator FollowPath()
        {
            if (path.Length > 0)
            {
                Vector3 currentWaypoint = path[0];
                targetIndex = 0;
                while (true)
                {
                    if (transform.position == currentWaypoint)
                    {
                        targetIndex++;
                        if (targetIndex >= path.Length)
                        {
                            yield break;
                        }
                        currentWaypoint = path[targetIndex];
                    }
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    yield return null;
                }
            }

        }
    }

}