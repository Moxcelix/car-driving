using UnityEngine;

namespace Core.CarAI
{
    public class TargetFollow
    {
        private readonly Transform _carBody;

        private Transform _target;

        public float ForwardAmount { get; private set; }

        public float TurnAmount { get; private set; }

        public bool UseReverse { get; set; } = false;

        public TargetFollow(Transform carBody)
        {
            _carBody = carBody;
        }

        public void Update()
        {
            var directionToMovePosition = (_target.position - _carBody.position).normalized;
            var dot = Vector3.Dot(_carBody.forward, directionToMovePosition);


            var angleToDirection =
                Vector3.SignedAngle(_carBody.forward, directionToMovePosition, Vector3.up);

            var forwardAmount = dot > 0 || !UseReverse ? 1.0f : -1.0f;
            var turnAmount = angleToDirection > 0 ? 1.0f : -1.0f;

            ForwardAmount = forwardAmount;
            TurnAmount = turnAmount;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}