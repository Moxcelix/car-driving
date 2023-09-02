namespace Core.Car
{
    public class CarController
    {
        private readonly IControls _controls;

        private readonly Car _car;

        public Car Car => _car;

        public bool IsAvailable { get; set; }

        public CarController(IControls _controls, Car _car)
        {
            this._controls = _controls;
            this._car = _car;

            IsAvailable = true;
        }

        public void Update()
        {
            if (!IsAvailable) return;

            if(_car == null) return;

            _car.GasPedal.Value = _controls.Gas;
            _car.BrakePedal.Value = _controls.Brake;
            _car.SteeringWheel.Steer(_controls.SteerDelta);
            _car.Transmission.SwitchMode(_controls.TransmissionMode);
            _car.ParkingBrake.Switch(_controls.ParkingBrakeSwitch);
            _car.TurnLights.SwitchBlinker(_controls.BlinkerState);
            _car.TurnLights.SwitchEmergency(_controls.EmergencySwitch);
            _car.HeadLights.SwitchHighLight(_controls.HighLightSwitch);
            _car.Engine.Starter.SwitchState(_controls.EngineState);
        }
    }
}