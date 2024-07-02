using UnityEngine;
using Core.CarAI.Agent;
using Core.CarAI.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Core.CarAI
{
    public class TargetFinder : ITargetFinder
    {
        private PathFinder _pathFinder;
        private List<Node> _path;

        private int _currentPathIndex = 0;

        public NodeConnection CurrentNodeConnection { get; private set; }

        public bool IsDone => _currentPathIndex == _path.Count - 1;

        public void SetDestination(Node startNode, Node endNode)
        {
            _pathFinder = new PathFinder();
            _path = _pathFinder.CreatePath(startNode, endNode);
        }

        public Transform GetTarget()
        {
            return _path[_currentPathIndex].transform;
        }

        public void NextTarget()
        {
            if (_currentPathIndex >= _path.Count - 1)
            {
                return;
            }

            var connections = from node in _path[_currentPathIndex].Nodes
                              where node.Target == _path[_currentPathIndex + 1]
                              select node;

            CurrentNodeConnection = connections.FirstOrDefault();

            _currentPathIndex++;
        }

        public void ResetTarget()
        {
            _currentPathIndex = 0;
        }
    }
}
