using Core.Car;
using Core.CarAI.Agent;
using UnityEngine;
using System.Collections;

public class CarDriverAI : MonoBehaviour, IControls
{
    private readonly float _steerSpeed = 1f;

    private readonly SmoothPressing _gasSmoothPressing = new(0.5f, 0.5f);
    private readonly SmoothPressing _brakeSmoothPressing = new(1f, 1.5f);

    [SerializeField] private Car _car;
    [SerializeField] private Transform _target;

    private CarController _carController;
    private TargetFollow _targetFollow;
    private Driver _driver;

    private float _gas = 0.0f;
    private float _brake = 0.0f;

    public float Gas { get; private set; }

    public float Brake { get; private set; }

    public float SteerDelta { get; private set; }

    public bool ParkingBrakeSwitch { get; private set; }

    public bool EmergencySwitch { get; private set; }

    public bool HighLightSwitch { get; private set; }

    public bool EngineState { get; private set; }

    public TransmissionMode TransmissionMode { get; private set; }

    public BlinkerState BlinkerState { get; private set; }

    private void Awake()
    {
        _targetFollow = new TargetFollow(_car.transform);
        _targetFollow.SetTarget(_target);
        _targetFollow.UseReverse = true;

        _driver = new Driver(_targetFollow, null);

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
        _carController.Update();
        _driver.Update(_car.GetSpeed());

        if(_brake > 0)
        {
            _brakeSmoothPressing.Press(_brake, Time.deltaTime);
        }
        else
        {
            _brakeSmoothPressing.Release(Time.deltaTime);
        }

        Gas = _gasSmoothPressing.Value;
        Brake = _brakeSmoothPressing.Value;
    }

    private IEnumerator TESTAI()
    {
        EngineState = true;
        ParkingBrakeSwitch = false;
        SteerDelta = 0;

        _brake = 1;

        yield return new WaitForSeconds(2.0f);

        TransmissionMode = TransmissionMode.DRIVING;

        yield return new WaitForSeconds(2.0f);

        _brake = 0;

        while (true)
        {
            SteerDelta =
                (_driver.TurnAmount - _car.SteeringWheel.TurnAmount) *
                Time.unscaledDeltaTime * _steerSpeed;

            _brake = _driver.Brake;

            _gas = Mathf.Abs(_driver.Acceleration);

            if (_gas > 0)
            {
                _gasSmoothPressing.Press(_gas, Time.unscaledDeltaTime);
            }
            else
            {
                _gasSmoothPressing.Release(Time.unscaledDeltaTime);
            } 

            if (_driver.Acceleration < 0 && TransmissionMode == TransmissionMode.DRIVING)
            {
                _brake = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.REVERSE;

                _brake = 0;
            }

            if (_driver.Acceleration >= 0 && TransmissionMode != TransmissionMode.DRIVING)
            {
                _brake = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.DRIVING;

                _brake = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
