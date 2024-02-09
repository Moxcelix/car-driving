using System;
using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class Node : MonoBehaviour
    {
        [SerializeField] private Node[] _nodes;

        public Node[] Nodes => _nodes;

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
                    var direction = node.transform.position - transform.position;

                    Gizmos.DrawLine(
                        transform.position + direction.normalized * radius, 
                        node.transform.position - direction.normalized * radius);

                    var right = Quaternion.LookRotation(direction) *
                        Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * Vector3.forward;
                    var left = Quaternion.LookRotation(direction) *
                        Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * Vector3.forward;
                    Gizmos.DrawRay(node.transform.position - direction.normalized * radius,
                        right * arrowHeadLength);
                    Gizmos.DrawRay(node.transform.position - direction.normalized * radius,
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