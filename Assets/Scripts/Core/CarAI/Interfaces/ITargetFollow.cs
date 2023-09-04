using UnityEngine;

namespace Core.CarAI
{
    public interface ITargetFollow
    {
        public float TurnAmount { get; }

        public float ForwardAmount { get; }

        public void SetTarget(Transform target);

        public void Update();
    }
}