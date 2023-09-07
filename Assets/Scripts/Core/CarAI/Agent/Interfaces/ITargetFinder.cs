using UnityEngine;

namespace Core.CarAI
{
    public interface ITargetFinder
    {
        public Transform GetTarget();
    }
}