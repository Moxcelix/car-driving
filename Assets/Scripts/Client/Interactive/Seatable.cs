using System;
using UnityEngine;

[RequireComponent(typeof(Core.Entity.SeatPlace))]
[RequireComponent(typeof(Core.Car.Seat))]
public class Seatable : MonoBehaviour, IInteractive
{
    [SerializeField] private Core.Car.Door _door;
    [SerializeField] private string _hintText;

    private Core.Entity.SeatPlace _playerSeat;
    private Core.Car.Seat _carSeat;
    private AvatarController _avatarControler;

    public Action<AvatarController> OnAvatarSitting;
    public Action<AvatarController> OnAvatarLeaving;

    public string Hint => _hintText;

    private void Awake()
    {
        _playerSeat = GetComponent<Core.Entity.SeatPlace>();
        _carSeat = GetComponent<Core.Car.Seat>();

        _playerSeat.OnSitting += Refresh;
        _playerSeat.OnLeaving += Refresh;
    }

    private void OnDestroy()
    {
        _playerSeat.OnSitting -= Refresh;
        _playerSeat.OnLeaving -= Refresh;
    }

    private void Update()
    {
        _playerSeat.IsLocked = _door.State
            == Core.Car.IOpenable.OpenState.CLOSED;
    }

    private void Refresh()
    {
        _carSeat.IsTaken = _playerSeat.IsTaken;

        if (_avatarControler is not null && !_playerSeat.IsTaken)
        {
            OnAvatarLeaving?.Invoke(_avatarControler);

            _avatarControler = null;
        }
    }

    public bool IsInteractable(AvatarController avatarController)
    {
        return _playerSeat.IsInteractable(
            avatarController.EntityController.EntityBody);
    }

    public void Interact(AvatarController avatarController)
    {
        if (_playerSeat.Take(avatarController.
            EntityController.EntityBody))
        {
            _avatarControler = avatarController;

            OnAvatarSitting?.Invoke(_avatarControler);
        }
    }
}
