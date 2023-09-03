using UnityEngine;

namespace Core.CarAI
{
    public class TargetFollow
    {
        private readonly Transform _carTransform;

        private Transform _target;

        private readonly float _reachedDistance = 10f;
        private readonly float _maxAngle = 30.0f;

        public float ForwardAmount { get; private set; }

        public float TurnAmount { get; private set; }

        public bool UseReverse { get; set; } = false;

        public TargetFollow(Transform carBody)
        {
            _carTransform = carBody;
        }

        public void Update()
        {
            var distance = Vector3.Distance(_carTransform.position, _target.position);

            var forwardAmount = 0.0f;
            var turnAmount = 0.0f;

            if (distance > _reachedDistance)
            {
                var directionToMovePosition = (_target.position - _carTransform.position).normalized;
                var dot = Vector3.Dot(_carTransform.forward, directionToMovePosition);

                var angleToDirection =
                    Vector3.SignedAngle(_carTransform.forward, directionToMovePosition, Vector3.up);

                forwardAmount = dot > 0 || !UseReverse ? 1.0f : -1.0f;
                turnAmount = Mathf.Clamp(angleToDirection / _maxAngle, -1.0f, 1.0f);
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