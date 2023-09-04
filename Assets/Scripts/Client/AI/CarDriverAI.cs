using Core.Car;
using Core.CarAI;
using UnityEngine;
using System.Collections;

public class CarDriverAI : MonoBehaviour, IControls
{
    private readonly float _steerSpeed = 1f;
    private readonly SmoothPressing _smoothPressing = new(0.5f, 0.5f);

    [SerializeField] private Car _car;
    [SerializeField] private Transform _target;

    private CarController _carController;
    private TargetFollow _targetFollow;
    private Driver _driver;


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
        _targetFollow.UseReverse = false;

        _driver = new Driver(_targetFollow);

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
        _driver.Update();
    }

    private IEnumerator TESTAI()
    {
        EngineState = true;
        Brake = 1;
        ParkingBrakeSwitch = false;
        SteerDelta = 0;

        yield return new WaitForSeconds(2.0f);

        TransmissionMode = TransmissionMode.DRIVING;

        yield return new WaitForSeconds(2.0f);

        Brake = 0;

        while (true)
        {
            SteerDelta =
                (_driver.TurnAmount - _car.SteeringWheel.TurnAmount) *
                Time.unscaledDeltaTime * _steerSpeed;

            Gas = Mathf.Abs(_driver.Acceleration) * 0.1f;

            if (_driver.Acceleration < 0 && TransmissionMode == TransmissionMode.DRIVING)
            {
                Brake = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.REVERSE;

                Brake = 0;
            }

            if (_driver.Acceleration >= 0 && TransmissionMode != TransmissionMode.DRIVING)
            {
                Brake = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.DRIVING;

                Brake = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
