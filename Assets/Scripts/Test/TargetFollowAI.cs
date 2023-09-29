using Core.CarAI.Agent;
using Core.Neural;
using UnityEngine;

namespace Test
{
    public class TargetFollowAI : ITargetFollow
    {
        private readonly float _maxAngle = 30.0f;

        private Transform _carTransform;

        private Transform _target;

        private NeuralNetwork _neuralNetwork;

        private float _prevForwardAmount = 0;
        private float _prevTurnAmount = 0;

        public float TurnAmount { get; private set; }

        public float ForwardAmount { get; private set; }

        public bool TargetReached { get; private set; }

        public bool UseReverse { get; set; } = false;

        public TargetFollowAI(NeuralNetwork neuralNetwork)
        {
            _neuralNetwork = neuralNetwork;
        }

        public void SetTarget(Transform target)
        {
            _carTransform = target;
        }

        public void Update(float steerAngle, float reachedDistance)
        {
            var distance = Vector3.Distance(_carTransform.position, _target.position);

            var directionToMovePosition = (_target.position - _carTransform.position).normalized;
            var angleToDirection = steerAngle +
                    Vector3.SignedAngle(_carTransform.forward, directionToMovePosition, Vector3.up);

            var turnAmount = Mathf.Clamp(angleToDirection / _maxAngle, -1.0f, 1.0f);
            var forwardAmount = 0.0f;

            TargetReached = distance < reachedDistance;

            if (!TargetReached)
            {
                var dot = Vector3.Dot(_carTransform.forward, directionToMovePosition);

                forwardAmount = dot > 0 || !UseReverse ? 1.0f : -1.0f;
            }

            var result = _neuralNetwork.FeedForward(new float[] { 
                forwardAmount, 
                turnAmount, 
                _prevForwardAmount, 
                _prevTurnAmount});

            _prevForwardAmount = forwardAmount;
            _prevTurnAmount = turnAmount;

            ForwardAmount = result[0];
            TurnAmount = result[1];
        }
    }
}