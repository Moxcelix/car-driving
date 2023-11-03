using UnityEngine;
using Core.Car;

[RequireComponent(typeof(Car))]
public class CarStatistics : MonoBehaviour
{
    private Car _car;

    private void Awake()
    {
        _car = GetComponent<Car>();
    }

    private StatisticsRecord Collect()
    {
        return new StatisticsRecord(
            _car.GetSpeed() * 3.6f,
            _car.GasPedal.Value,
            _car.BrakePedal.Value,
            _car.Engine.RPM,
            _car.SteeringWheel.TurnAmount);
    }
}
