using UnityEngine;

namespace Core.Car
{
    public class Computer
    {
        private const float speedUpdateInterval = 0.5f;
        private const float consumptionUpdateInterval = 0.7f;

        private readonly Car _car;
        private readonly Counter _speedUpdateCounter;
        private readonly Counter _consumptionUpdateCounter;
        private readonly Mileage _mileage;

        public float Speed { get; private set; }

        public AutomaticTransmissionMode TransmissionMode { get; private set; }

        public int Gear { get; private set; }

        public float Consumption { get; private set; }

        public float Mileage { get; private set; }

        public Computer(Car car)
        {
            _car = car;

            _mileage = new Mileage();
            _speedUpdateCounter = new Counter(speedUpdateInterval);
            _consumptionUpdateCounter = new Counter(consumptionUpdateInterval);
            _speedUpdateCounter.OnCounterEnd += UpdateSpeed;
            _consumptionUpdateCounter.OnCounterEnd += UpdateConsumption;
        }

        ~Computer()
        {
            _speedUpdateCounter.OnCounterEnd -= UpdateSpeed;
            _consumptionUpdateCounter.OnCounterEnd -= UpdateConsumption;
        }

        public void Update()
        {
            TransmissionMode = _car.Transmission.Mode;
            Gear = _car.Transmission.CurrentGear + 1;
            Mileage = _mileage.Amount;

            _speedUpdateCounter.Update(Time.unscaledDeltaTime);
            _consumptionUpdateCounter.Update(Time.unscaledDeltaTime);
            _mileage.Update(Mathf.Abs(_car.GetSpeed()) /
                1000.0f *  Time.unscaledDeltaTime);
        }

        private void UpdateSpeed()
        {
            Speed = (int)Mathf.Abs(_car.GetSpeed() * 3.6f);
        }

        private void UpdateConsumption()
        {
            Consumption = _car.Engine.Consumption;
        }
    }
}
