using Core.Car;
using Core.CarAI;
using Core.CarAI.Agent;
using Core.CarAI.Navigation;
using Core.Damage;
using Core.CarMLAgents;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LearningCarAgent))]
public class CarDriverAI : MonoBehaviour, IControls
{
    public delegate void RestartDelegate();
    public event RestartDelegate OnRestart;

    private readonly float _steerSpeed = 2f;

    private readonly SmoothPressing _gasSmoothPressing = new(0.5f, 100.5f);
    private readonly SmoothPressing _brakeSmoothPressing = new(1f, 100.5f);

    [SerializeField] private Car _car;
    [SerializeField] private Damageable _damageable;
    [SerializeField] private DynamicHitTester[] _hitTesters;
    // Test
    [SerializeField] private Transform _target;
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _endNode;

    private CarController _carController;
    private TargetFollow _targetFollow;
    private TargetFinder _targetFinder;
    private Driver _driver;

    private LearningCarAgent _learningAgent;

    private float _gas = 0.0f;
    private float _brake = 0.0f;

    private Vector3 _startPosition;

    public IControls.ToogleSwitchDelegate EngineSwitch { get; set; }

    public IControls.BlinkerStateSwitchDelegate BlinkerStateSwitch { get; set; }

    public IControls.ToogleSwitchDelegate HighLightSwitch { get; set; }

    public IControls.ToogleSwitchDelegate EmergencySwitch { get; set; }

    public IControls.ToogleSwitchDelegate ParkingBrakeSwitch { get; set; }

    public IControls.ToogleSwitchDelegate TransmissionSelectorUp { set; get; }

    public IControls.ToogleSwitchDelegate TransmissionSelectorDown { set; get; }

    public IControls.ToogleSwitchDelegate TransmissionSelectorRight { set; get; }

    public IControls.ToogleSwitchDelegate TransmissionSelectorLeft { set; get; }

    public float Gas { get; private set; }

    public float Brake { get; private set; }

    public float Clutch { get; private set; }

    public float SteerDelta { get; private set; }


    private void Awake()
    {
        _learningAgent = GetComponent<LearningCarAgent>();

        _targetFollow = new TargetFollow(_car.transform);
        _targetFinder = new TargetFinder();
        _targetFinder.SetDestination(_startNode, _endNode);
        _targetFollow.UseReverse = false;

        _driver = new Driver(_targetFollow, _targetFinder, _hitTesters);

        _carController = new CarController(this, _car);
        _carController.IsAvailable = true;

        _damageable.OnDamage += _learningAgent.Bump;
        _damageable.OnDamage += Restart;

        _startPosition = _car.transform.position;

    }

    private void OnDestroy()
    {
        _damageable.OnDamage -= _learningAgent.Bump;
        _damageable.OnDamage -= Restart;
    }

    private void Start()
    {
        // TEST
        StartCoroutine(MainAI());
    }

    private void Update()
    {
        UpdateHits();
        UpdateSteer();

        var speed = _car.GetSpeed();
        var destinationDistance = speed * speed / 10.0f + 1.0f;

        if (Vector3.Distance(
            _targetFinder.GetTarget().position,
            _car.transform.position) < destinationDistance)
        {
            _targetFinder.NextTarget();
            _learningAgent.PositionReach();
            _learningAgent.EndEpisode();

            if (_targetFinder.IsDone)
            {
                Restart();
            }
        }

        _carController.Update();
        _driver.Update(_car.SteeringWheel.TurnAmount, speed);

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

        Gas = _gasSmoothPressing.Value;
        Brake = _brakeSmoothPressing.Value;
    }

    private void Restart()
    {
        OnRestart?.Invoke();

        _targetFinder.ResetTarget();
        _car.transform.position = _startPosition;
    }

    private void UpdateHits()
    {
        var hits = new Hit[_hitTesters.Length];

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i] = new Hit()
            {
                Distance = 
                    _hitTesters[i].HitDistance /
                    _hitTesters[i].MaxDistance
            };
        }

        _learningAgent.UpdateHits(hits);
    }

    private void UpdateSteer()
    {
        _learningAgent.UpdateSteer(_car.SteeringWheel.TurnAmount);
    }

    private float GetSteerDelta(float deltaTime)
    {
        return (
            _driver.TurnAmount - 
            _car.SteeringWheel.TurnAmount) *
                deltaTime * _steerSpeed;
    }

    private IEnumerator MainAI()
    {
        yield return StartCar();

        while (true)
        {
            SteerDelta = GetSteerDelta(Time.unscaledDeltaTime);

            /*switch (_driver.Mode)
            {
                case Mode.Driving:
                    _gas = _learningAgent.Gas;
                    _brake = _learningAgent.Brake;

                    if (_driver.Acceleration < 0 &&
                        _car.Transmission.Mode == AutomaticTransmissionMode.DRIVING)
                    {
                        _brake = 1;
                        yield return new WaitForSeconds(2.0f);
                        TransmissionModeSwitch?.Invoke(AutomaticTransmissionMode.REVERSE);
                        _brake = 0;
                    }

                    if (_driver.Acceleration >= 0 &&
                        _car.Transmission.Mode != AutomaticTransmissionMode.DRIVING)
                    {
                        _brake = 1;
                        yield return new WaitForSeconds(2.0f);
                        TransmissionModeSwitch?.Invoke(AutomaticTransmissionMode.DRIVING);
                        _brake = 0;
                    }
                    break;
                case Mode.Accident:
                    _gas = 0;
                    _brake = 1;

                    if (_car.Transmission.Mode != AutomaticTransmissionMode.PARKING)
                    {
                        TransmissionModeSwitch?.Invoke(AutomaticTransmissionMode.PARKING);
                    }
                    if (!_car.TurnLights.EmergencyState)
                    {
                        EmergencySwitch?.Invoke();
                    }

                    break;
            }*/

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator StartCar()
    {
        _brake = 1;

        yield return new WaitForSeconds(0.1f);

        if (_car.Engine.Starter.State == EngineState.STOPED)
        {
            EngineSwitch?.Invoke();

            yield return new WaitForSeconds(2.0f);
        }

        if (_car.ParkingBrake.State == ParkingBrakeState.RAISED)
        {
            ParkingBrakeSwitch?.Invoke();

            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < 3; i++)
        {
            TransmissionSelectorDown?.Invoke();

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);

        _brake = 0;
    }
}
