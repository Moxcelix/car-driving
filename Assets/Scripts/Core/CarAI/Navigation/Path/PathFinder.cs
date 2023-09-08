using Core.CarAI.Navigation;
using UnityEngine;

namespace Core.CarAI
{
    public class PathFinder
    {
        public float GetWeight(Node a, Node b)
        {
            return Vector3.Distance(a.transform.position, b.transform.position);
        }
    }
}