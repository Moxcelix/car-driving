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

    private void Awake()
    {
        _car = GetComponent<Car>();

        _webCarController = new WebCarController(_car, _sendTimeout, _receiveTimeout);
    }
}
