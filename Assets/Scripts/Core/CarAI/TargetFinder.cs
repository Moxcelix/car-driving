using UnityEngine;
using Core.CarAI.Agent;
using Core.CarAI.Navigation;
using System.Collections.Generic;

namespace Core.CarAI
{
    public class TargetFinder : ITargetFinder
    {
        private PathFinder _pathFinder;
        private List<Node> _path;

        private int _currentPathIndex = 0;

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
            if(_currentPathIndex >= _path.Count - 1)
            {
                return;
            }

            _currentPathIndex++;
        }
    }
}
