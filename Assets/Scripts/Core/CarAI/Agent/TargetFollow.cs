using UnityEngine;

namespace Core.CarAI.Agent
{
    public class TargetFollow : ITargetFollow
    {
        private readonly Transform _carTransform;

        private readonly float _maxAngle = 30.0f;

        private Transform _target;

        public float ForwardAmount { get; private set; }

        public float TurnAmount { get; private set; }

        public bool TargetReached { get; private set; }

        public bool UseReverse { get; set; } = false;


        public TargetFollow(Transform carBody)
        {
            _carTransform = carBody;
        }

        public void Update(float reachedDistance)
        {
            var distance = Vector3.Distance(_carTransform.position, _target.position);

            var directionToMovePosition = (_target.position - _carTransform.position).normalized;
            var angleToDirection =
                    Vector3.SignedAngle(_carTransform.forward, directionToMovePosition, Vector3.up);

            var turnAmount = Mathf.Clamp(angleToDirection / _maxAngle, -1.0f, 1.0f);
            var forwardAmount = 0.0f;

            TargetReached = distance < reachedDistance;

            if (!TargetReached)
            {
                var dot = Vector3.Dot(_carTransform.forward, directionToMovePosition);

                forwardAmount = dot > 0 || !UseReverse ? 1.0f : -1.0f;
            }

            ForwardAmount = forwardAmount;
            TurnAmount = turnAmount;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}