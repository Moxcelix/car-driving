using Core.Car;
using Core.Carsharing;
using Core.Web;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class WebHandler : MonoBehaviour
{
    private Car _car;
    private WebCarController _webCarController;
    private Telemetry _telemetry;

    [SerializeField] private int _sendTimeout = 1000;
    [SerializeField] private int _receiveTimeout = 1000;

    [SerializeField] private string _sendUrl;
    [SerializeField] private string _receiveUrl;

    [SerializeField] private int _id;

    private readonly WaitForSeconds _delay = new(1f);

    private readonly string _protocol = "http";

    private void Start()
    {
        _car = GetComponent<Car>();

        var receiveUrl = GetGetUrl();
        var sendUrl = GetSendUrl();

        _telemetry = new Telemetry(_car, _id);

        _webCarController = new WebCarController(
            _sendTimeout,
            _receiveTimeout,
            sendUrl,
            receiveUrl);

        Debug.Log(receiveUrl);
        Debug.Log(sendUrl);

        _webCarController.OpenLock += OpenLock;
        _webCarController.CloseLock += CloseLock;
        _webCarController.OpenUnlock += OpenUnlock;

        _webCarController.Start();

        StartCoroutine(UpdateTelemetry());
    }

    private string GetGetUrl()
    {
        return _protocol + "://" + _receiveUrl + "?car_id=" + _id.ToString();
    }

    private string GetSendUrl()
    {
        return _protocol + "://" + _sendUrl;
    }

    private void OnApplicationQuit()
    {
        _webCarController.Stop();
    }

    private IEnumerator UpdateTelemetry()
    {
        while (true)
        {
            _webCarController.SetSendData(_telemetry.GetData());

            yield return _delay;
        }
    }

    private void CloseLock()
    {
        _car.CentralLocking.Locked = true;
        _car.Immobilizer.IsActive = true;
    }

    private void OpenLock()
    {
        _car.CentralLocking.Locked = false;
        _car.Immobilizer.IsActive = true;
    }

    private void OpenUnlock()
    {
        _car.CentralLocking.Locked = false;
        _car.Immobilizer.IsActive = false;
    }
}
