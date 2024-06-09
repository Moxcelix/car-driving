using UnityEngine;
using Core.Car;
using System.IO;
using System.Collections;
using System;


[RequireComponent(typeof(Car))]
[RequireComponent(typeof(CarID))]
public class TransmissionStatistics : MonoBehaviour
{
    private const string path = "transmission stats";
    private const float precision = 0.0001f;

    private Car _car;
    private CarID _carID;

    private readonly WaitForSeconds _sleep = new(0.1f);

    private void Awake()
    {
        _car = GetComponent<Car>();
        _carID = GetComponent<CarID>();
    }


    private void Start()
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        StartCoroutine(RecordCycle());
    }


    private (
        float speed,
        float rpm,
        float gas, 
        float brake, 
        int gear, 
        float consumption,
        float torque)
        Collect()
    {
        var speed = _car.GetSpeed();
        return (
            speed < precision ? 0 : speed,
            _car.Engine.RPM,
            _car.GasPedal.Value,
            _car.BrakePedal.Value,
            _car.Transmission.CurrentGear,
            _car.Computer.Consumption,
            _car.Engine.Torque);
    }

    private void WriteStatistics(
        (float speed,
        float rpm,
        float gas,
        float brake,
        int gear,
        float consumption,
        float torque) record,
        StreamWriter streamWriter)
    {
        streamWriter.WriteLine(
            $"{record.speed} " +
            $"{record.rpm} " +
            $"{record.gas} " +
            $"{record.brake} " +
            $"{record.gear} " +
            $"{record.consumption} " +
            $"{record.torque}");
    }

    private string GetFileName()
    {
        return $"transmission {_carID.ID} {DateTime.Now:yyyy-MM-dd HH-mm-ss}.stt";
    }

    private IEnumerator RecordCycle()
    {
        var fileName = GetFileName();
        var filePath = Path.Combine(path, fileName);

        using StreamWriter outputFile = new StreamWriter(filePath, true);
        outputFile.WriteLine("Speed RPM Gas Brake GearNumber Consumption Torque");

        while (true)
        {
            var statistics = Collect();
            WriteStatistics(statistics, outputFile);

            yield return _sleep;
        }
    }
}
