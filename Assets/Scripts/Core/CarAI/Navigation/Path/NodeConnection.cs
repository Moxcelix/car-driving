using UnityEngine;

namespace Core.CarAI.Navigation
{
    [System.Serializable]

    public class NodeConnection
    {
        [SerializeField] private Node _target;
        [Range(0, 1)][SerializeField] private float _priority = 0.5f;

        public Node Target => _target;
        public float Priority => _priority;
    }
}