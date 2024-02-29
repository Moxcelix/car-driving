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
        public bool SmoothSync { get; set; } = false;

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
            UpdateLighs();
            UpdateDashboard();
            UpdateComputer();
            UpdateCentralLocking();
            UpdateImmobilizer();

            if (Syncable)
            {
                return;
            }

            HandleWheels();
            HandleEngine();
            HandleBrakeing();
        }

        public void Synchronize(CarState state, float deltaTime = 1)
        {
            if (!Syncable)
            {
                _state = null;

                return;
            }

            _state = SmoothSync && _state != null ? CarState.Lerp(_state, state, deltaTime) : state;

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
            _parkingBrake.LoadSyncState(_state.ParkingBrake);
            // Sync steering wheel.
            _steeringWheel.TurnAmount = _state.TurnAmount;
            // Sync blinkers.
            _turnLights.LoadSyncState(_state.BlinkerState, _state.Emergency);
            // Sync head lights.
            _headLights.LoadSyncState(_state.HighLight);
            // Sync engine.
            _engine.Starter.LoadSyncState(_state.EngineState);
            _engine.LoadSyncState(_state.RPM);
            // Sync transmission.
            _transmissionSelector.LoadSyncState(_state.TransmissionSelectorPosition);
            // Sync wheels.
            _frontLeftWheel.LoadSyncState(_state.LeftFrontWheelPosition,
                _state.LeftFrontWheelRotation);
            _frontRightWheel.LoadSyncState(_state.RightFrontWheelPosition,
                _state.RightFrontWheelRotation);
            _rearLeftWheel.LoadSyncState(_state.LeftRearWheelPosition,
                _state.LeftRearWheelRotation);
            _rearRightWheel.LoadSyncState(_state.RightRearWheelPosition,
                _state.RightRearWheelRotation);
            // Sync this.
            transform.SetPositionAndRotation(
                new Vector3(state.BodyPosition.x, state.BodyPosition.y, state.BodyPosition.z),
                Quaternion.Euler(state.BodyRotation.x, state.BodyRotation.y, state.BodyRotation.z));
        }

        public CarState GetSyncState()
        {
            if (Syncable)
            {
                return null;
            }

            var doorStates = new bool[_doors.Length];

            for (int i = 0; i < _doors.Length; i++)
            {
                doorStates[i] = _doors[i].State == IOpenable.OpenState.OPEN;
            }

            var blinkerState = _turnLights.GetSyncState();
            var frontLeftWheelState = _frontLeftWheel.GetSyncState();
            var frontRightWheelState = _frontRightWheel.GetSyncState();
            var rearLeftWheelState = _rearLeftWheel.GetSyncState();
            var rearRightWheelState = _rearRightWheel.GetSyncState();

            (float x, float y, float z) Vector3ToTuple(Vector3 vector) => (vector.x, vector.y, vector.z);

            var state = new CarState
            {
                RPM = _engine.GetSyncState(),
                Speed = GetSpeed(),
                Brake = _brakePedal.Value,
                Gas = _gasPedal.Value,
                Clutch = (_transmission as ManualTransmission)?
                            .ClutchPedal.Value ?? 0.0f,
                TurnAmount = _steeringWheel.TurnAmount,
                EngineState = _engine.Starter.GetSyncState(),
                ParkingBrake = _parkingBrake.GetSyncState(),
                HighLight = _headLights.GetSyncState(),
                TransmissionSelectorPosition = _transmissionSelector.GetSyncState(),
                BlinkerState = blinkerState.Item1,
                Emergency = blinkerState.Item2,
                LeftFrontWheelPosition = frontLeftWheelState.position,
                LeftFrontWheelRotation = frontLeftWheelState.rotation,
                RightFrontWheelPosition = frontRightWheelState.position,
                RightFrontWheelRotation = frontRightWheelState.rotation,
                LeftRearWheelPosition = rearLeftWheelState.position,
                LeftRearWheelRotation = rearLeftWheelState.rotation,
                RightRearWheelPosition = rearRightWheelState.position,
                RightRearWheelRotation = rearRightWheelState.rotation,
                BodyPosition = Vector3ToTuple(transform.position),
                BodyRotation = Vector3ToTuple(transform.rotation.eulerAngles),
            };

            return state;
        }

        public float GetSpeed()
        {
            return Syncable ? _state?.Speed ?? 0.0f : Vector3.Dot(
                    _rigidbody.velocity,
                    _rigidbody.transform.forward);
        }

        private float GetWheelsRPM()
        {
            return (_frontLeftWheel.RPM + _frontRightWheel.RPM) * 0.5f;
        }

        private void HandleWheels()
        {
            _frontLeftWheel.TurnAmount = _steeringWheel.TurnAmount;
            _frontRightWheel.TurnAmount = _steeringWheel.TurnAmount;

            _frontLeftWheel.Handle();
            _frontRightWheel.Handle();
            _rearLeftWheel.Handle();
            _rearRightWheel.Handle();
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

        private void UpdateDashboard()
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

        private void UpdateLighs()
        {
            _headLights.Enabled = _engine.Enabled;

            _headLights.Update();
            _turnLights.Update();
            _stopLights.SetLight(BrakePedal.Value > 0);
            _backLights.SetLight(
                _engine.Enabled &&
                Transmission.IsReverse);
        }

        private void UpdateComputer()
        {
            _computer.Update();
        }

        private void UpdateCentralLocking()
        {
            _centralLocking.Update();
        }

        private void UpdateImmobilizer()
        {
            _immobilizer.Update();
        }
    }
}