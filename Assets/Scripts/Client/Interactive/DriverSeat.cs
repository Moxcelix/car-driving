using System;
using Core.Car;
using UnityEngine;

[RequireComponent(typeof(CarSeatPlace))]
public class DriverSeat : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private ObservableManager _carMirrors;

    private CarSeatPlace _seatable;

    private void Awake()
    {
        _seatable = GetComponent<CarSeatPlace>();

        _seatable.OnAvatarSitting += ProvideCarHandling;
        _seatable.OnAvatarLeaving += DepriveCarHandling;

    }

    private void Start()
    {
        _carMirrors.SetActive(false);
    }

    private void OnDestroy()
    {
        _seatable.OnAvatarSitting -= ProvideCarHandling;
        _seatable.OnAvatarLeaving -= DepriveCarHandling;
    }

    private void ProvideCarHandling(AvatarController avatarController)
    {
        Debug.Log($"[{DateTime.Now}] -> {avatarController}" +
            $" has access to control {_car}");

        avatarController.ProvideCarHandling(_car);

        if (avatarController.AvatarType == AvatarType.OBSERVED)
        {
            _carMirrors.SetActive(true);
        }
    }

    private void DepriveCarHandling(AvatarController avatarController)
    {
        Debug.Log($"[{DateTime.Now}] -> {avatarController}" +
            $" has lost to control {_car}");

        avatarController.DepriveCarHandling();

        if (avatarController.AvatarType == AvatarType.OBSERVED)
        {
            _carMirrors.SetActive(false);
        }
    }
}
