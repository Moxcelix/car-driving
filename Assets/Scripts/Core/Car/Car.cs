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
        [SerializeField] private SteeringWheel _steeringWheel;
        [SerializeField] private Tachometer _tachometer;
        [SerializeField] private Speedometer _speedometer;
        [SerializeField] private ParkingBrake _parkingBrake;
        [SerializeField] private Pedal _gasPedal;
        [SerializeField] private Pedal _BrakePedal;

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

        public Pedal GasPedal => _gasPedal;
        public Pedal BrakePedal => _BrakePedal;
        public ParkingBrake ParkingBrake => _parkingBrake;
        public SteeringWheel SteeringWheel => _steeringWheel;
        public Engine Engine => _engine;
        public Transmission Transmission => _transmission;
        public TurnLights TurnLights => _turnLights;
        public HeadLights HeadLights => _headLights;
        public Computer Computer => _computer;
        public CentralLocking CentralLocking => _centralLocking;
        public Immobilizer Immobilizer => _immobilizer;

        public Door[] Doors => _doors;

        private void Awake()
        {
            _transmission.Initialize();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = _centerOfMass.localPosition;

            _computer = new Computer(this);
            _centralLocking = new CentralLocking(_doors);
            _immobilizer = new Immobilizer(_engine);
        }

        private void FixedUpdate()
        {
            HandleSteering();
            HandleEngine();
            HandleDashboard();
            HandleBrakeing();
            HandleLighs();
            HandleComputer();
            HandleCentralLocking();
            HandleImmobilizer();
        }

        public float GetSpeed()
        {
            return Vector3.Dot(
                    _rigidbody.velocity,
                    _rigidbody.transform.forward);
        }

        public Vector3 GetVelocity()
        {
            return _rigidbody.velocity;
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

            _transmission.Lock = !_engine.Enabled || !_BrakePedal.IsPressed;
            _frontLeftWheel.TransmitTorque(
                _transmission.Torque - resistance);
            _frontRightWheel.TransmitTorque(
                _transmission.Torque - resistance);

            _engine.Update(_gasPedal.Value,
                _transmission.RPM,
                _transmission.Load,
                Time.fixedDeltaTime);

            _transmission.Update(
                _gasPedal.Value,
                _engine.Torque,
                _engine.RPM,
                wheelsRPM,
                GetSpeed() * 3.6f,
                Time.fixedDeltaTime);
        }

        private void HandleDashboard()
        {
            _speedometer.UpdateValue(GetSpeed() * 3.6f);
            _tachometer.UpdateValue(_engine.RPM);
        }

        private void HandleBrakeing()
        {
            var frontBrakeValue =
                (_BrakePedal.Value +
                _transmission.Brake) * _brakeForce;

            var rearBrakeValue =
                (_BrakePedal.Value +
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
                Transmission.Mode == AutomaticTransmissionMode.REVERSE);
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