using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Yavuz.PathFind
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequests = new Queue<PathRequest>();
        PathRequest currentRequest;

        static PathRequestManager instance;

        PathFinding pathFinding;

        bool isProcessing;

        private void Awake()
        {
            instance = this;
            pathFinding = GetComponent<PathFinding>();
        }
        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
            instance.pathRequests.Enqueue(newRequest);
            instance.TryProcessNext();

        }

        private void TryProcessNext()
        {
            if (!isProcessing && pathRequests.Count > 0)
            {
                currentRequest = pathRequests.Dequeue();
                isProcessing = true;
                pathFinding.StartFindPath(currentRequest.pathStart, currentRequest.pathEnd);
            }
        }
        public void FinishedProcessingPath(Vector3[] path, bool success)
        {

            currentRequest.callback(path, success);
            isProcessing = false;
            TryProcessNext();
        }

        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }
        }
    }
}
