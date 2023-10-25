namespace Core.Car
{
    public class Computer
    {
        private readonly Car _car;

        public float Speed { get; private set; }

        public TransmissionMode TransmissionMode { get; private set; }

        public int Gear { get; private set; }

        public void Update()
        {
            Speed = (int)_car.GetSpeed();
            TransmissionMode = _car.Transmission.Mode;
            Gear = _car.Transmission.CurrentGear;
        }
    }
}
