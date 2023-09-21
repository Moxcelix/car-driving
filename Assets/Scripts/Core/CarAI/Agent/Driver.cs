using System.Linq;
using UnityEngine;

namespace Core.CarAI.Agent
{
    public class Driver : IDriver
    {
        private readonly ITargetFollow _targetFollow;

        private readonly ITargetFinder _targetFinder;

        private readonly IHitTester[] _hitTesters;

        public float Acceleration { get; private set; }

        public float Brake { get; private set; }

        public float TurnAmount { get; private set; }

        public float SteerSpeed { get; private set; }

        public Mode Mode { get; private set; }

        public Driver(
            ITargetFollow targetFollow,
            ITargetFinder targetFinder,
            IHitTester[] hitTesters)
        {
            _targetFollow = targetFollow;
            _targetFinder = targetFinder;
            _hitTesters = hitTesters;

            Mode = Mode.Driving;
        }

        public void MakeAccident()
        {
            Mode = Mode.Accident;
        }

        public void Update(float speed)
        {
            var minBrakeDistance = speed / 2.0f + 2.0f;
            var hits = from item in _hitTesters
                       where item.HitDistance < minBrakeDistance
                       select item;

            switch (Mode)
            {
                case Mode.Driving:
                    _targetFollow.Update(speed * 0.5f + 7.0f);

                    var determinatedDirection = DeterminateDirection(
                        new Vector3(
                            _targetFollow.TurnAmount, 0,
                            _targetFollow.ForwardAmount));

                    TurnAmount = _targetFollow.TurnAmount;
                    Brake = _targetFollow.TargetReached || determinatedDirection ? 1.0f : 0.0f;
                    Acceleration = Brake > 0 ? 0.0f : _targetFollow.ForwardAmount * 0.2f;
                    break;
                case Mode.Idling:
                    Acceleration = 0;
                    TurnAmount = 0;
                    Brake = speed > 0 ? 1.0f : 0.0f;
                    break;
                case Mode.Accident:
                    Acceleration = 0;
                    TurnAmount = 0;
                    Brake = speed > 0.1f + float.Epsilon ? 1.0f : 0.0f;
                    break;
                case Mode.Parking:
                    TurnAmount = 0;
                    Acceleration = 0;
                    break;
            }
        }

        private bool DeterminateDirection(Vector3 direction)
        {
            var minAngle = 30.0f;

            foreach(var item in _hitTesters)
            {
                if (item.IsHited && Vector3.Angle(item.Direction, direction) <= minAngle) 
                {
                    return true;
                }
            }

            return false;
        }
    }
}