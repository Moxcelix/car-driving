using UnityEngine;
using Core.Car;
using System.IO;
using System.Collections;

[RequireComponent(typeof(Car))]
public class TransmissionStatistics : MonoBehaviour
{
    private const string path = "transmission stats";

    private Car _car;
    private AutomaticTransmission _transmission;

    private readonly WaitForSeconds _sleep = new(0.1f);

    private void Awake()
    {
        _car = GetComponent<Car>();
        _transmission = _car.Transmission as AutomaticTransmission;
    }


    private void Start()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        StartCoroutine(RecordCycle());
    }


    private (float speed, float rpm, float gas, float brake, int gear) Collect()
    {
        return (_car.GetSpeed(), _car.Engine.RPM, _car.GasPedal.Value, _car.BrakePedal.Value, _car.Transmission.CurrentGear);
    }

    private void WriteStatistics(
        (float speed, float rpm, float gas, float brake, int gear) record, string file)
    {
        using StreamWriter outputFile = new StreamWriter(file, true);

        outputFile.WriteLine(
            $"{record.speed} " +
            $"{record.rpm} " +
            $"{record.gas} " +
            $"{record.brake} " +
            $"{record.gear}");
    }

    private string GetFileName()
    {
        return $"transmission.stt";
    }

    private IEnumerator RecordCycle()
    {
        while (true)
        {
            if (_transmission.Mode == AutomaticTransmissionMode.MANUAL)
            {
                var fileName = GetFileName();
                var filePath = Path.Combine(path, fileName);
                var statistics = Collect();

                WriteStatistics(statistics, filePath);
            }

            yield return _sleep;
        }
    }
}
