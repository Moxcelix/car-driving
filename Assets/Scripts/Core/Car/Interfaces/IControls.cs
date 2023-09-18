namespace Core.Car
{
    public interface IControls
    {
        public delegate void ToogleSwitchDelegate();
        public delegate void BlinkerStateSwitchDelegate(BlinkerState state);
        public delegate void TransmissionModeSwitchDelegate(TransmissionMode mode);

        public ToogleSwitchDelegate EngineSwitch { set; }
        public  ToogleSwitchDelegate HighLightSwitch { set; }
        public  ToogleSwitchDelegate EmergencySwitch { set; }
        public  ToogleSwitchDelegate ParkingBrakeSwitch { set; }
        public  BlinkerStateSwitchDelegate BlinkerStateSwitch { set; }
        public  TransmissionModeSwitchDelegate TransmissionModeSwitch { set; }

        public float Gas { get; }
        public float Brake { get; }
        public float SteerDelta { get; }

        public void Update();
    }
}