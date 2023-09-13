using Core.CarAI.Navigation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.CarAI
{
    public class PathFinder
    {
        /// <summary>
        /// Temporarily returns the length of the shortest path.
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="endNode"></param>
        /// <returns></returns>
        public float CreatePath(Node startNode, Node endNode)
        {
            var nodes = GetAllNodes(startNode);
            var s = new List<Node>() { startNode };
            var weights = new Dictionary<Node, float>() { { startNode, 0.0f } };
            var minInputNode = new Dictionary<Node, Node>();

            while (s.Count < nodes.Count)
            {
                var currentNode = s.Last();
                var minNode = (Node)null;

                foreach (var node in nodes)
                {
                    if (s.Contains(node))
                    {
                        continue;
                    }

                    var weight = GetWeight(currentNode, node);

                    weights[node] = weights.ContainsKey(node) ?
                        Mathf.Min(weights[node], weight + weights[currentNode]) :
                        weight + weights[currentNode];

                    Debug.Log($"{currentNode}, {node} = {weights[node]}");

                    if (minNode == null ||
                        weights[node] < weights[currentNode] + GetWeight(currentNode, minNode))
                    {
                        minNode = node;
                    }
                }

                s.Add(minNode);
            }

            foreach(var node in nodes)
            {
                foreach (var nextNode in node.Nodes)
                {
                    if (!minInputNode.ContainsKey(nextNode) ||
                        weights[node] < weights[minInputNode[nextNode]])
                    {
                        minInputNode[nextNode] = node;

                        Debug.Log(node);
                    }
                }
            }

            var path = new List<Node>();

            for (var node = endNode; node != startNode; node = minInputNode[node])
            {
                path.Insert(0, node);
            }

            path.Insert(0, startNode);

            Debug.Log(path.Count);

            return weights.ContainsKey(endNode) ?
                weights[endNode] :
                float.PositiveInfinity;
        }

        public float GetWeight(Node a, Node b)
        {
            if (a.Nodes.Contains(b))
            {
                return Vector3.Distance(a.transform.position, b.transform.position);
            }

            return float.PositiveInfinity;
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