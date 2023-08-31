namespace Core.Car
{
    public class CarController
    {
        private readonly IControls _controls;

        private Car _car;

        public bool IsAvailable { get; set; }

        public Car Car => _car;

        public CarController(IControls _controls)
        {
            this._controls = _controls;

            IsAvailable = true;
        }

        public void SetCar(Car car)
        {
            if(car == null)
            {
                throw new System.NullReferenceException();
            }

            this._car = car;
        }

        public void RemoveCar()
        {
            this._car = null;
        }

        public void Update()
        {
            if (!IsAvailable) return;

            if(_car == null) return;

            _car.GasPedal.Value = _controls.Gas;
            _car.BreakPedal.Value = _controls.Break;
            _car.SteeringWheel.Steer(_controls.SteerDelta);
            _car.Transmission.SwitchMode(_controls.TransmissionMode);
            _car.ParkingBreak.Switch(_controls.ParkingBreakSwitch);
            _car.TurnLights.SwitchBlinker(_controls.BlinkerState);
            _car.TurnLights.SwitchEmergency(_controls.EmergencySwitch);
            _car.HeadLights.SwitchHighLight(_controls.HighLightSwitch);
            _car.Engine.Starter.SwitchState(_controls.EngineState);
        }
    }
}