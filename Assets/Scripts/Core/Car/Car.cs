using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Wheel _frontRightWheel;
        [SerializeField] private Wheel _frontLeftWheel;
        [SerializeField] private Wheel _rearRightWheel;
        [SerializeField] private Wheel _rearLeftWheel;
        [SerializeField] private float _brakeForce;

        [Header("Controls")]
        [SerializeField] private TransmissionSelector _transmissionSelector;
        [SerializeField] private SteeringWheel _steeringWheel;
        [SerializeField] private Tachometer _tachometer;
        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private ParkingBrake _parkingBrake;
        [SerializeField] private Pedal _gasPedal;
        [SerializeField] private Pedal _brakePedal;

        [Header("Lighting")]
        [SerializeField] private TurnLights _turnLights;
        [SerializeField] private HeadLights _headLights;
        [SerializeField] private LightGroup _stopLights;
        [SerializeField] private LightGroup _backLights;

        [Header("Engine")]
        [SerializeField] private Engine _engine;
        [SerializeField] private Transmission _transmission;

        [Header("Doors")]
        [SerializeField] private Door[] _doors;

        private Rigidbody _rigidbody;
        private Computer _computer;
        private CentralLocking _centralLocking;
        private Immobilizer _immobilizer;
        private CarState _state;

        public bool Syncable { get; set; } = false;

        public Pedal GasPedal => _gasPedal;
        public Pedal BrakePedal => _brakePedal;
        public ParkingBrake ParkingBrake => _parkingBrake;
        public SteeringWheel SteeringWheel => _steeringWheel;
        public Engine Engine => _engine;
        public Transmission Transmission => _transmission;
        public TurnLights TurnLights => _turnLights;
        public HeadLights HeadLights => _headLights;
        public Computer Computer => _computer;
        public CentralLocking CentralLocking => _centralLocking;
        public Immobilizer Immobilizer => _immobilizer;
        public TransmissionSelector TransmissionSelector => _transmissionSelector;

        public Door[] Doors => _doors;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = _centerOfMass.localPosition;

            _computer = new Computer(this);
            _centralLocking = new CentralLocking(_doors);
            _immobilizer = new Immobilizer(_engine);
        }

        private void Update()
        {
            _rigidbody.isKinematic = Syncable;
        }

        private void FixedUpdate()
        {
            if (Syncable)
            {
                return;
            }

            HandleSteering();
            HandleEngine();
            HandleDashboard();
            HandleBrakeing();
            HandleLighs();
            HandleComputer();
            HandleCentralLocking();
            HandleImmobilizer();
        }

        public void Synchronize(CarState state)
        {
            if (!Syncable)
            {
                _state = null;

                return;
            }

            _state = state;

            // Sync gas pedal.
            _gasPedal.Value = _state.Gas;
            // Sync brake pedal.
            _brakePedal.Value = _state.Brake;
            // Sync clutch pedal.
            var manualTransmission = _transmission as ManualTransmission;

            if (manualTransmission != null)
            {
                manualTransmission.ClutchPedal.Value = _state.Clutch;
            }
            // Sync parking brake.
            if (_parkingBrake.State
                == ParkingBrakeState.LOWERED
                == _state.ParkingBrake)
            {
                _parkingBrake.Switch();
            }
            // Sync steering wheel.
            _steeringWheel.TurnAmount = _state.TurnAmount;
            // Sync dashboard.
            _speedometer.UpdateValue(_state.Speed * 3.6f);
            _tachometer.UpdateValue(_state.RPM);
            // Sync blinkers.
            _turnLights.SwitchBlinker(_state.BlinkerState);
            // Sync head lights.
            if (_headLights.LightState == HeadLightState.DIPPED == _state.HighLight)
            {
                _headLights.SwitchHighLight();
            }
            // Sync emergency.
            if (_turnLights.EmergencyState != _state.Emergency)
            {
                _turnLights.SwitchEmergency();
            }
            // Sync engine.
            if (_engine.Starter.State == EngineState.STOPED == _state.EngineState)
            {
                _engine.Starter.SwitchState();
            }
            _engine.LoadState(_state.RPM);
            // Sync transmission.
            _transmissionSelector.LoadState(_state.TransmissionSelectorPosition);
            // Sync wheels;
            _frontLeftWheel.TurnAmount = _state.TurnAmount;
            _frontRightWheel.TurnAmount = _state.TurnAmount;
            _frontLeftWheel.LoadState(_state.LeftFrontWheel);
            _frontRightWheel.LoadState(_state.RightFrontWheel);
            _rearLeftWheel.LoadState(_state.LeftRearWheel);
            _rearRightWheel.LoadState(_state.RightRearWheel);
            // Sync doors.
            for (int i = 0; i < _doors.Length; i++)
            {
                _doors[i].LoadState(_state.DoorStates[i]);
            }
        }

        public CarState GetState()
        {
            if (Syncable)
            {
                return null;
            }

            var state = new CarState
            {
                RPM = _engine.RPM,
                Speed = GetSpeed(),
                Brake = _brakePedal.Value,
                Gas = _gasPedal.Value,
                Clutch = (_transmission as ManualTransmission)?
                            .ClutchPedal.Value ?? 0.0f,
                EngineState = _engine.Starter.State == EngineState.STARTED,
                ParkingBrake = _parkingBrake.State == ParkingBrakeState.RAISED,
                HighLight = _headLights.LightState == HeadLightState.HIGH,
                TransmissionSelectorPosition = _transmissionSelector.Position,
                BlinkerState = _turnLights.BlinkerState,
                Emergency = _turnLights.EmergencyState,
                LeftFrontWheel = _frontLeftWheel.transform,
                RightFrontWheel = _frontRightWheel.transform,
                LeftRearWheel = _rearLeftWheel.transform,
                RightRearWheel = _rearRightWheel.transform
            };

            var doorStates = new bool[_doors.Length];

            for (int i = 0; i < _doors.Length; i++)
            {
                doorStates[i] = _doors[i].State == IOpenable.OpenState.OPEN;
            }

            return state;
        }

        public float GetSpeed()
        {
            return Syncable ? _state.Speed : Vector3.Dot(
                    _rigidbody.velocity,
                    _rigidbody.transform.forward);
        }

        private float GetWheelsRPM()
        {
            return (_frontLeftWheel.RPM + _frontRightWheel.RPM) * 0.5f;
        }

        private void HandleSteering()
        {
            _frontLeftWheel.TurnAmount = _steeringWheel.TurnAmount;
            _frontRightWheel.TurnAmount = _steeringWheel.TurnAmount;
        }

        private void HandleEngine()
        {
            var resistance = 0;
            var wheelsRPM = GetWheelsRPM();

            _frontLeftWheel.TransmitTorque(
                _transmission.Torque - resistance);
            _frontRightWheel.TransmitTorque(
                _transmission.Torque - resistance);

            _engine.Update(_gasPedal.Value,
                _transmission.RPM,
                _transmission.Load,
                Time.fixedDeltaTime);

            _transmission.SetValues(
                _engine.Torque,
                _engine.RPM,
                wheelsRPM);
        }

        private void HandleDashboard()
        {
            _speedometer.UpdateValue(GetSpeed() * 3.6f);
            _tachometer.UpdateValue(_engine.RPM);
        }

        private void HandleBrakeing()
        {
            var frontBrakeValue =
                (_brakePedal.Value +
                _transmission.Brake) * _brakeForce;

            var rearBrakeValue =
                (_brakePedal.Value +
                _parkingBrake.Brake) * _brakeForce;

            _frontRightWheel.Brake(frontBrakeValue);
            _frontLeftWheel.Brake(frontBrakeValue);
            _rearRightWheel.Brake(rearBrakeValue);
            _rearLeftWheel.Brake(rearBrakeValue);
        }

        private void HandleLighs()
        {
            _headLights.Enabled = _engine.Enabled;

            _headLights.Update();
            _turnLights.Update();
            _stopLights.SetLight(BrakePedal.Value > 0);
            _backLights.SetLight(
                _engine.Enabled &&
                Transmission.IsReverse);
        }

        private void HandleComputer()
        {
            _computer.Update();
        }

        private void HandleCentralLocking()
        {
            _centralLocking.Update();
        }

        private void HandleImmobilizer()
        {
            _immobilizer.Update();
        }
    }
}