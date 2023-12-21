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

    private readonly WaitForSeconds _delay = new WaitForSeconds(1f);

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

        _webCarController.Start();

        StartCoroutine(UpdateTelemetry());
    }

    private string GetGetUrl()
    {
        return "http://" + _receiveUrl + "?car_id=" + _id.ToString();
    }

    private string GetSendUrl()
    {
        return "http://" + _sendUrl;
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
}
