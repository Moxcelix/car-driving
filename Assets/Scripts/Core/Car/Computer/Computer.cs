using UnityEngine;

namespace Core.Car
{
    public class Computer
    {
        private readonly Car _car;

        public float Speed { get; private set; }

        public TransmissionMode TransmissionMode { get; private set; }

        public int Gear { get; private set; }

        public Computer(Car car)
        {
            _car = car;
        }

        public void Update()
        {
            Speed = (int)Mathf.Abs(_car.GetSpeed() * 3.6f);
            TransmissionMode = _car.Transmission.Mode;
            Gear = _car.Transmission.CurrentGear + 1;
        }
    }
}
