using Core.Car;
using Core.Web;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class WebHandler : MonoBehaviour
{
    private Car _car;
    private WebCarController _webCarController;

    [SerializeField] private int _sendTimeout = 1000;
    [SerializeField] private int _receiveTimeout = 1000;

    [SerializeField] private string _sendUrl;
    [SerializeField] private string _receiveUrl;

    [SerializeField] private int _id;

    private void Start()
    {
        _car = GetComponent<Car>();

        var receiveUrl = GetGetUrl();
        var sendUrl = GetSendUrl();

        _webCarController = new WebCarController(
            _car,
            _sendTimeout, 
            _receiveTimeout,
            sendUrl,
            receiveUrl);

        Debug.Log(receiveUrl);
        Debug.Log(sendUrl);

        _webCarController.Start();
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
}
