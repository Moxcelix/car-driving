using UnityEngine;

namespace Core.CarAI.Agent
{
    public interface ITargetFollow
    {
        public float TurnAmount { get; }


        public float ForwardAmount { get; }


        public bool TargetReached { get; }  

        public void SetTarget(Transform target);

        public void Update(float turnAmount, float reachedDistance);
    }
}