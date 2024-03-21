using UnityEngine;

namespace Core.Car
{
    public class BrakeSystem
    {
        private const float InputSensetivity = 0.05f;
        private const float SpeedSensetivity = 0.05f;
        private const float RpmSensetivity = 0.05f;
        private const float Freequency = 1.00f;

        private readonly Wheel[] _wheels;
        private readonly float _brakeForce;

        private float _timer = 0f;

        public float ABS { get; private set; }

        public BrakeSystem(Wheel[] wheels, float brakeForce)
        {
            _wheels = wheels;
            _brakeForce = brakeForce;
        }

        public void Update(float input, float speed, float deltaTime)
        {
            ABS = 0;

            _timer += deltaTime;

            if (_timer > Mathf.PI * 2.0f)
            {
                _timer = 0f;
            }

            foreach (var wheel in _wheels)
            {
                var press = input;

                if (input > InputSensetivity &&
                    speed > SpeedSensetivity &&
                    wheel.RPM < RpmSensetivity)
                {
                    press = input * Mathf.Sin(_timer * Freequency);

                    ABS = press;
                }

                wheel.Brake(press * _brakeForce);
            }
        }
    }
}
