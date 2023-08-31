using Core.Car;
using UnityEngine;

public class CarDriverAI : MonoBehaviour
{
    [SerializeField] private Car _car;

    private CarController _carController;
    private CarControlsAI _carControls;

    private void Awake()
    {
        _carControls = new CarControlsAI();
        _carController = new CarController(_carControls);
        _carController.SetCar(_car);
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
    }
}
