using Core.Car;
using Core.CarAI;
using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Transform _target;

    private CarController _carController;
    private CarControlsAI _carControls;
    private TargetFollow _targetFollow;

    private void Awake()
    {
        _targetFollow = new TargetFollow(_car.transform);
        _targetFollow.SetTarget(_target);
        _targetFollow.UseReverse = true;

        _carControls = new CarControlsAI(_targetFollow);
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
