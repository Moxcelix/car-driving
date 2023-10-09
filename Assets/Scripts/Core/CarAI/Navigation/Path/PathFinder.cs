using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class PathFinder
    {
        private List<Node> _nodes;
        private Dictionary<Node, float> _marks;
        private Dictionary<Node, Node> _minInputNode;

        public List<Node> CreatePath(Node startNode, Node endNode)
        {
            RecalculateMarks(startNode);
            RecalculateMinInputNodes();

            var path = MakeMinPath(startNode, endNode);

            return _marks.ContainsKey(endNode) ? path : null;
        }

        public float GetWeight(Node a, Node b)
        {
            if (a.Nodes.Contains(b))
            {
                return Vector3.Distance(a.transform.position, b.transform.position);
            }

            return float.PositiveInfinity;
        }

        private void RecalculateMarks(Node startNode)
        {
            var s = new List<Node>() { startNode };

            _nodes = GetAllNodes(startNode);
            _marks = new Dictionary<Node, float>() { { startNode, 0.0f } };
            _minInputNode = new Dictionary<Node, Node>();

            while (s.Count < _nodes.Count)
            {
                var currentNode = s.Last();
                var minNode = (Node)null;

                foreach (var node in _nodes)
                {
                    if (s.Contains(node))
                    {
                        continue;
                    }

                    var weight = GetWeight(currentNode, node);

                    _marks[node] = _marks.ContainsKey(node) ?
                        Mathf.Min(_marks[node], weight + _marks[currentNode]) :
                        weight + _marks[currentNode];

                    Debug.Log($"{currentNode}, {node} = {_marks[node]}");

                    if (minNode == null ||
                        _marks[node] < _marks[currentNode] + GetWeight(currentNode, minNode))
                    {
                        minNode = node;
                    }
                }

                s.Add(minNode);
            }
        }

        private void RecalculateMinInputNodes()
        {
            foreach (var node in _nodes)
            {
                foreach (var nextNode in node.Nodes)
                {
                    if (!_minInputNode.ContainsKey(nextNode) ||
                        _marks[node] < _marks[_minInputNode[nextNode]])
                    {
                        _minInputNode[nextNode] = node;
                    }
                }
            }
        }

        private List<Node> MakeMinPath(Node startNode, Node endNode)
        {
            var path = new List<Node>();

            for (var node = endNode; node != startNode; node = _minInputNode[node])
            {
                path.Insert(0, node);
            }

            path.Insert(0, startNode);

            return path;
        }

        private List<Node> GetAllNodes(Node node)
        {
            var nodes = new List<Node>();
            var queue = new List<Node>
            {
                node
            };

            while (queue.Count > 0)
            {
                var currentNode = queue.Last();

                nodes.Add(currentNode);
                queue.AddRange(
                    from toQueue
                    in currentNode.Nodes
                    where !nodes.Contains(toQueue)
                    select toQueue);

                queue.Remove(currentNode);
            }

            return nodes;
        }
    }
}