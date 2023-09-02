using UnityEngine;

namespace Core.CarAI
{
    public class Driver
    {
        private readonly TargetFollow _targetFollow;

        public float Acceleration { get; private set; }

        public float Brake { get; private set; }

        public float TurnAmount { get; private set; }

        public float SteerSpeed { get; private set; }

        public Driver(Transform carBody) 
        {
            _targetFollow = new TargetFollow(carBody);
        }
    }
}