using UnityEngine;

namespace Core.CarAI.Agent
{
    public class TargetFollowPID : ITargetFollow
    {
        private readonly Transform _carTransform;

        private readonly float _maxAngle = 30.0f;

        private Transform _target;

        private readonly PIDController _forwardController =
            new PIDController(1.0f, 0.2f, 0.2f, 0.2f, // magig numbers
            PIDController.DerivativeMeasurement.Velocity);

        private readonly PIDController _turnController =
            new PIDController(1.0f, 0.2f, 0.2f, 0.2f, // magig numbers
            PIDController.DerivativeMeasurement.Velocity);

        public float ForwardAmount { get; private set; }

        public float TurnAmount { get; private set; }

        public bool TargetReached { get; private set; }

        public bool UseReverse { get; set; } = false;

        public TargetFollowPID(Transform carBody)
        {
            _carTransform = carBody;
        }

        public void Update(float reachedDistance)
        {
            var distance = Vector3.Distance(_carTransform.position, _target.position);

            var directionToMovePosition = (_target.position - _carTransform.position).normalized;
            var angleToDirection =
                    Vector3.SignedAngle(_carTransform.forward, directionToMovePosition, Vector3.up);

            var turnAmount = _turnController.UpdateAngle(Time.unscaledDeltaTime, 0, angleToDirection);
            var forwardAmount = 0.0f;

            TargetReached = distance < reachedDistance;

            if (!TargetReached)
            {
                var dot = Vector3.Dot(_carTransform.forward, directionToMovePosition);

                forwardAmount = -Mathf.Clamp(
                    _forwardController.Update(
                        Time.unscaledDeltaTime, 
                        dot, 0), -1.0f, 1.0f);

                if (!UseReverse)
                {
                    forwardAmount = Mathf.Abs(forwardAmount);
                }
            }

            Debug.Log(turnAmount);

            ForwardAmount = forwardAmount;
            TurnAmount = turnAmount;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }
    }
}