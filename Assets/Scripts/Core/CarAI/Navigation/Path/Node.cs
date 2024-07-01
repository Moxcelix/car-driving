using System;
using System.Linq;
using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private NodeConnection[] _nodes;

        public Node[] Nodes { get; private set; }

        private void Awake()
        {
            Nodes = (from node in _nodes select node.Target).ToArray();
        }

        private void OnDrawGizmos()
        {
            const float radius = 0.3f;
            const float arrowHeadLength = 1.5f;
            const float arrowHeadAngle = 30f;

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, radius);

            foreach (var node in _nodes)
            {
                try
                {
                    var direction = node.Target.transform.position - transform.position;

                    Gizmos.DrawLine(
                        transform.position + direction.normalized * radius, 
                        node.Target.transform.position - direction.normalized * radius);

                    var right = Quaternion.LookRotation(direction) *
                        Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
                    var left = Quaternion.LookRotation(direction) *
                        Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
                    Gizmos.DrawRay(node.Target.transform.position - direction.normalized * radius,
                        right * arrowHeadLength);
                    Gizmos.DrawRay(node.Target.transform.position - direction.normalized * radius,
                        left * arrowHeadLength);
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            const float radius = 0.3f;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
}