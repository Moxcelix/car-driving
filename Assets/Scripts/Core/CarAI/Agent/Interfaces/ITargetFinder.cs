using UnityEngine;

namespace Core.CarAI.Agent
{
    public interface ITargetFinder
    {
        public Transform GetTarget();

        public void NextTarget();
    }
}