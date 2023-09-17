namespace Core.Car
{
    public interface IControls
    {
        public delegate void ToogleSwitchDelegate(bool state);
        public delegate void BlinkerStateSwitchDelegate(BlinkerState state);
        public delegate void TransmissionModeSwitchDelegate(TransmissionMode mode);

        public event ToogleSwitchDelegate EngineSwitch;
        public event ToogleSwitchDelegate HighLightSwitch;
        public event ToogleSwitchDelegate EmergencySwitch;
        public event ToogleSwitchDelegate ParkingBrakeSwitch;
        public event BlinkerStateSwitchDelegate BlinkerStateSwitch;
        public event TransmissionModeSwitchDelegate TransmissionModeSwitch;

        public float Gas { get; }
        public float Brake { get; }
        public float SteerDelta { get; }
    }
}