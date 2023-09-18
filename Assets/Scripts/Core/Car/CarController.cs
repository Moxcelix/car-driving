namespace Core.Car
{
    public class CarController
    {
        private readonly IControls _controls;

        private readonly Car _car;

        private bool _closed = false;

        public Car Car => _car;

        public bool IsAvailable { get; set; }

        public CarController(IControls controls, Car car)
        {
            this._controls = controls;
            this._car = car;

            controls.TransmissionModeSwitch = car.Transmission.SwitchMode;
            controls.ParkingBrakeSwitch = car.ParkingBrake.Switch;
            controls.BlinkerStateSwitch = car.TurnLights.SwitchBlinker;
            controls.EmergencySwitch = car.TurnLights.SwitchEmergency;
            controls.HighLightSwitch = car.HeadLights.SwitchHighLight;
            controls.EngineSwitch = car.Engine.Starter.SwitchState;

            IsAvailable = true;
        }

        ~CarController()
        {
            if (_closed)
            {
                return;
            }

            Close();
        }

        public void Close()
        {
            _controls.TransmissionModeSwitch = null;
            _controls.ParkingBrakeSwitch = null;
            _controls.BlinkerStateSwitch = null;
            _controls.EmergencySwitch = null;
            _controls.HighLightSwitch = null;
            _controls.EngineSwitch = null;

            _closed = true;
        }

        public void Update()
        {
            if (!IsAvailable) return;

            _controls.Update();

            _car.GasPedal.Value = _controls.Gas;
            _car.BrakePedal.Value = _controls.Brake;
            _car.SteeringWheel.Steer(_controls.SteerDelta);
        }
    }
}