using Core.Car;
using Core.Web;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class WebHandler : MonoBehaviour
{
    private Car _car;
    private WebCarController _webCarController;

    private void Awake()
    {
        var sendTimeout = 10000;
        var receiveTimeout = 10000;

        _car = GetComponent<Car>();

        _webCarController = new WebCarController(_car, sendTimeout, receiveTimeout);
    }
}
