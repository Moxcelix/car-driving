using Core.Car;
using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Car))]
[RequireComponent(typeof(CarID))]
public class CarStatistics : MonoBehaviour
{
    private const string path = "stats";

    private Car _car;
    private CarID _carID;
    private CarDriverAI _driverAI;
    private int _iteration;

    private readonly WaitForSeconds _sleep = new WaitForSeconds(0.1f);

    private void Awake()
    {
        _car = GetComponent<Car>();
        _carID = GetComponent<CarID>();
        _driverAI = GetComponentInChildren<CarDriverAI>();
        _iteration = 0;

        _driverAI.OnRestart += NextIteration;
    }

    private void OnDestroy()
    {
        _driverAI.OnRestart -= NextIteration;
    }
    private void Start()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        StartCoroutine(RecordCycle());
    }

    private void NextIteration()
    {
        _iteration++;
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

    private void WriteStatistics(StatisticsRecord record, string file)
    {
        using StreamWriter outputFile = new StreamWriter(file, true);

        outputFile.WriteLine(
            $"{record.Speed} " +
            $"{record.Gas} " +
            $"{record.Brake} " +
            $"{record.RPM} " +
            $"{record.SteerAmount}");
    }

    private string GetFileName()
    {
        return $"{_carID.ID}_{_iteration}.stt";
    }

    private IEnumerator RecordCycle()
    {
        while (true)
        {
            var fileName = GetFileName();
            var filePath = Path.Combine(path, fileName);
            var statistics = Collect();

            WriteStatistics(statistics, filePath);

            yield return _sleep;
        }
    }
}
