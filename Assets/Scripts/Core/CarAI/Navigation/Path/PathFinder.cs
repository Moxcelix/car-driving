using Core.CarAI.Navigation;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.CarAI
{
    public class PathFinder
    {
        public void CreatePath(Node startNode, Node endNode)
        {
            var queue = new List<Node>();




        }

        public float GetWeight(Node a, Node b)
        {
            return Vector3.Distance(a.transform.position, b.transform.position);
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