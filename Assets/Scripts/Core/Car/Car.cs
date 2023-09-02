using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        public const float c_velocityEps = 0.01f;
        [Header("Physics")]
        [SerializeField] private Transform _centerOfMass;
        [SerializeField] private Wheel _frontRightWheel;
        [SerializeField] private Wheel _frontLeftWheel;
        [SerializeField] private Wheel _rearRightWheel;
        [SerializeField] private Wheel _rearLeftWheel;
        [SerializeField] private float _BrakeForce;
        [SerializeField] private float _maxSpeed;

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

        private Rigidbody _rigidbody;

        public Pedal GasPedal => _gasPedal;
        public Pedal BrakePedal => _BrakePedal;
        public ParkingBrake ParkingBrake => _parkingBrake;
        public SteeringWheel SteeringWheel => _steeringWheel;
        public Engine Engine => _engine;
        public Transmission Transmission => _transmission;
        public TurnLights TurnLights => _turnLights;
        public HeadLights HeadLights => _headLights;

        private void Awake()
        {
            _engine.Initialize();
            _transmission.Initialize();

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = _centerOfMass.localPosition;
        }

        private void FixedUpdate()
        {
            HandleSteering();
            HandleEngine();
            HandleDashboard();
            HandleBrakeing();
            HandleLighs();
        }

        public float GetVelocity()
        {
            var project = Vector3.Project(_rigidbody.velocity,
                _rigidbody.transform.forward);
            var movementSign = Mathf.Sign(project.x /
                _rigidbody.transform.forward.x);
            var v = project.magnitude;

            return v * movementSign;
        }

        private float GetResistanceForce()
        {
            var v = GetVelocity();
            var airResistance = _engine.MaxTorque / (_maxSpeed * _maxSpeed);

            if (Mathf.Abs(v) < c_velocityEps)
                v = 0;

            return airResistance * v * v * Mathf.Sign(v);
        }

        private float GetWheelsRPM()
        {
            return (_frontLeftWheel.RPM + _frontRightWheel.RPM) * 0.5f;
        }

        private void HandleSteering()
        {
            _frontLeftWheel.SteerAngle = _steeringWheel.SteerAngle;
            _frontRightWheel.SteerAngle = _steeringWheel.SteerAngle;
        }

        private void HandleEngine()
        {
            var resistance = GetResistanceForce();
            var wheelsRPM = GetWheelsRPM();

            _transmission.Lock = !_engine.Enabled || !_BrakePedal.IsPressed;
            _frontLeftWheel.TransmitTorque(
                _transmission.Torque - resistance);
            _frontRightWheel.TransmitTorque(
                _transmission.Torque - resistance);

            _engine.Update(_gasPedal.Value,
                _transmission.RPM,
                _transmission.Load);

            _transmission.Update(_engine.Torque,
                _engine.OutputRPM,
                wheelsRPM,
                GetVelocity() * 3.6f);
        }

        private void HandleDashboard()
        {
            _speedometer.UpdateValue(GetVelocity() * 3.6f);
            _tachometer.UpdateValue(_engine.RPM);
        }

        private void HandleBrakeing()
        {
            var frontBrakeValue =
                (_BrakePedal.Value +
                _transmission.Brake) * _BrakeForce;

            var rearBrakeValue =
                (_BrakePedal.Value +
                _parkingBrake.Brake) * _BrakeForce;

            _frontRightWheel.Brake(frontBrakeValue);
            _frontLeftWheel.Brake(frontBrakeValue);
            _rearRightWheel.Brake(rearBrakeValue);
            _rearLeftWheel.Brake(rearBrakeValue);
        }

        private void HandleLighs()
        {
            if (!_engine.Enabled &&
                _headLights.State == HeadLightState.HIGH)
            {
                _headLights.State = HeadLightState.DIPPED;
            }

            _headLights.Update();
            _turnLights.Update();
            _stopLights.SetLight(BrakePedal.Value > 0);
            _backLights.SetLight(
                _engine.Enabled &&
                Transmission.Mode == TransmissionMode.REVERSE);
        }
    }
}