using System;
using Core.Car;
using UnityEngine;

[RequireComponent(typeof(Seatable))]
public class DriverSeat : MonoBehaviour
{
    [SerializeField] private Car _car;

    private Seatable _seatable;

    private void Awake()
    {
        _seatable = GetComponent<Seatable>();

        _seatable.OnAvatarSitting += ProvideCarHandling;
        _seatable.OnAvatarLeaving += DepriveCarHandling;

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
    }

    private void DepriveCarHandling(AvatarController avatarController)
    {
        Debug.Log($"[{DateTime.Now}] -> {avatarController}" +
            $" has lost to control {_car}");

        avatarController.DepriveCarHandling();
    }

}
