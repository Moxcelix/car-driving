using Core.Car;
using Core.CarAI;
using Core.CarAI.Agent;
using Core.CarAI.Navigation;

using UnityEngine;
using System.Collections;
using Core.Damage;

public class CarDriverAI : MonoBehaviour, IControls
{
    private readonly float _steerSpeed = 2f;

    private readonly SmoothPressing _gasSmoothPressing = new(0.5f, 0.5f);
    private readonly SmoothPressing _brakeSmoothPressing = new(1f, 1.5f);

    [SerializeField] private Car _car;
    [SerializeField] private Damageable _damageable;
    [SerializeField] private HitTester[] _hitTesters;
    // Test
    [SerializeField] private Transform _target;
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _endNode;

    private CarController _carController;
    private TargetFollowPID _targetFollow;
    private TargetFinder _targetFinder;
    private Driver _driver;

    private float _gas = 0.0f;
    private float _brake = 0.0f;

    // PID Test.
    private readonly PIDController _pidController = 
        new PIDController(1.0f, 0.6f, 0.6f, 1.0f, // magig numbers
            PIDController.DerivativeMeasurement.Velocity);

    public IControls.ToogleSwitchDelegate EngineSwitch { get; set; }

    public IControls.BlinkerStateSwitchDelegate BlinkerStateSwitch { get; set; }

    public IControls.TransmissionModeSwitchDelegate TransmissionModeSwitch { get; set; }

    public IControls.ToogleSwitchDelegate HighLightSwitch { get; set; }

    public IControls.ToogleSwitchDelegate EmergencySwitch { get; set; }

    public IControls.ToogleSwitchDelegate ParkingBrakeSwitch { get; set; }

    public float Gas { get; private set; }

    public float Brake { get; private set; }

    public float SteerDelta { get; private set; }


    private void Awake()
    {
        _targetFollow = new TargetFollowPID(_car.transform);
        _targetFinder = new TargetFinder();
        _targetFinder.SetDestination(_startNode, _endNode);
        _targetFollow.UseReverse = false;

        _driver = new Driver(_targetFollow, _targetFinder, _hitTesters);

        _carController = new CarController(this, _car);
        _carController.IsAvailable = true;
    }

    private void Start()
    {
        // TEST
        StartCoroutine(TESTAI());
    }

    private void Update()
    {
        var speed = _car.GetSpeed();
        var destinationDistance = speed / 2.0f + 2.0f;

        if (Vector3.Distance(
            _targetFinder.GetTarget().position, 
            _car.transform.position) < destinationDistance)
        {
            _targetFinder.NextTarget();
        }

        _carController.Update();
        _driver.Update(speed);

        if (_brake > 0)
        {
            _brakeSmoothPressing.Press(_brake, Time.deltaTime);
        }
        else
        {
            _brakeSmoothPressing.Release(Time.deltaTime);
        }

        if (_gas > 0)
        {
            _gasSmoothPressing.Press(_gas, Time.deltaTime);
        }
        else
        {
            _gasSmoothPressing.Release(Time.deltaTime);
        }
    }

    void IControls.Update()
    {
        Gas = _gasSmoothPressing.Value;
        Brake = _brakeSmoothPressing.Value;
    }

    private IEnumerator TESTAI()
    {
        if(_car.Engine.Starter.State == EngineState.STOPED)
        {
            EngineSwitch?.Invoke();
        }

        if(_car.ParkingBrake.State == ParkingBrakeState.RAISED)
        {
            ParkingBrakeSwitch?.Invoke();
        }

        SteerDelta = 0;

        _brake = 1;

        yield return new WaitForSeconds(2.0f);

        TransmissionModeSwitch?.Invoke(TransmissionMode.DRIVING);

        yield return new WaitForSeconds(2.0f);

        _brake = 0;

        while (true)
        {
            SteerDelta = (_driver.TurnAmount - _car.SteeringWheel.TurnAmount)
                * Time.unscaledDeltaTime * _steerSpeed;

            _brake = _driver.Brake;

            _gas = Mathf.Abs(_driver.Acceleration);

            if (_damageable.IsDamaged)
            {
                _driver.MakeAccident();
            }

            switch (_driver.Mode)
            {
                case Mode.Driving:
                    if (
                        _driver.Acceleration < 0 &&
                        _car.Transmission.Mode == TransmissionMode.DRIVING)
                    {
                        _brake = 1;
                        yield return new WaitForSeconds(2.0f);
                        TransmissionModeSwitch?.Invoke(TransmissionMode.REVERSE);
                        _brake = 0;
                    }

                    if (_driver.Acceleration >= 0
                        && _car.Transmission.Mode != TransmissionMode.DRIVING)
                    {
                        _brake = 1;
                        yield return new WaitForSeconds(2.0f);
                        TransmissionModeSwitch?.Invoke(TransmissionMode.DRIVING);
                        _brake = 0;
                    }
                    break;
                case Mode.Accident:
                    if (_car.Transmission.Mode != TransmissionMode.PARKING)
                    {
                        TransmissionModeSwitch?.Invoke(TransmissionMode.PARKING);
                    }
                    if (!_car.TurnLights.EmergencyState)
                    {
                        EmergencySwitch?.Invoke();
                    }

                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
