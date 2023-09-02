using Core.Car;
using Core.CarAI;
using UnityEngine;

public class CarDriverAI : MonoBehaviour, IControls
{
    [SerializeField] private Car _car;
    [SerializeField] private Transform _target;

    private CarController _carController;
    private CarControlsAI _carControls;
    private TargetFollow _targetFollow;

    public float Gas { get; private set; }

    public float Break { get; private set; }

    public float SteerDelta { get; private set; }

    public bool ParkingBreakSwitch { get; private set; }

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

        _carControls = new CarControlsAI(_targetFollow, _car);
        _carController = new CarController(_carControls, _car);
        _carController.IsAvailable = true;
    }

    private void Start()
    {
        // TEST
        StartCoroutine(_carControls.TESTAI());
    }

    private void Update()
    {
        _carController.Update();
        _targetFollow.Update();
    }
}
