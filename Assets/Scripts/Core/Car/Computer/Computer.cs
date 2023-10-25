using UnityEngine;

namespace Core.Car
{
    public class Computer
    {
        private const float speedUpdateInterval = 0.5f;

        private readonly Car _car;
        private readonly Counter _speedUpdateCounter;

        public float Speed { get; private set; }

        public TransmissionMode TransmissionMode { get; private set; }

        public int Gear { get; private set; }

        public float Consumption { get; private set; }

        public Computer(Car car)
        {
            _car = car;

            _speedUpdateCounter = new Counter(speedUpdateInterval);
            _speedUpdateCounter.OnCounterEnd += UpdateSpeed;
        }

        ~Computer()
        {
            _speedUpdateCounter.OnCounterEnd -= UpdateSpeed;
        }

        public void Update()
        {
            TransmissionMode = _car.Transmission.Mode;
            Gear = _car.Transmission.CurrentGear + 1;
            Consumption = _car.GasPedal.Value;

            _speedUpdateCounter.Update(Time.fixedDeltaTime);
        }

        private void UpdateSpeed()
        {
            Speed = (int)Mathf.Abs(_car.GetSpeed() * 3.6f);
        }
    }
}
