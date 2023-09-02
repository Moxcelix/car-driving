namespace Core.Car
{
    public interface IControls
    {
        public float Gas { get; }
        public float Brake { get; }
        public float SteerDelta { get; }
        public bool EngineState { get; }
        public bool ParkingBrakeSwitch { get; }
        public bool EmergencySwitch { get; }
        public bool HighLightSwitch { get; }
        public BlinkerState BlinkerState { get; }
        public TransmissionMode TransmissionMode { get; }
    }
}