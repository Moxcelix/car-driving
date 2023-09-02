using Core.Car;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] private Car _car;

    [SerializeField] private EngineSound _engineSound;
    [SerializeField] private BlinkerSound _blinkerSound;
    [SerializeField] private TransmissionSound _transmissionSound;
    [SerializeField] private ParkingBrakeSound _parkingBrakeSound;
    [SerializeField] private DoorSound _doorSound;

    private void Awake()
    {
        _engineSound.Initialize(_car.Engine);
        _blinkerSound.Initialize(_car.TurnLights);
        _transmissionSound.Initialize(_car.Transmission);
        _parkingBrakeSound.Initialize(_car.ParkingBrake);
        _doorSound.Initialize();
    }

    private void OnDestroy()
    {
        _engineSound.Destroy();
        _blinkerSound.Destroy();
        _transmissionSound.Destroy();
        _parkingBrakeSound.Destroy();
        _doorSound.Destroy();
    }

    private void Update()
    {
        _engineSound.Update();
    }
}
