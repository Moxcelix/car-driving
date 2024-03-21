using Core.Car;
using System;
using UnityEngine;

[Serializable]
public class DoorSound
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _openDoor;
    [SerializeField] private AudioClip _closeDoor;
    [SerializeField] private AudioClip _slamLid;
    [SerializeField] private AudioClip _gaslift;

    [SerializeField] private Door[] _doors;
    [SerializeField] private Door _hood;
    [SerializeField] private Door _trunk;

    public void Initialize()
    {
        _hood.OnStateChange += PlayLidSound;
        _trunk.OnStateChange += PlayLidSound;

        for (int i = 0; i < _doors.Length; i++)
        {
            _doors[i].OnStateChange += PlayDoorSound;
        }
    }

    public void Destroy()
    {
        _hood.OnStateChange -= PlayLidSound;
        _trunk.OnStateChange -= PlayLidSound;

        for (int i = 0; i < _doors.Length; i++)
        {
            _doors[i].OnStateChange -= PlayDoorSound;
        }
    }

    private void PlayDoorSound(IOpenable.OpenState state)
    {
        switch (state)
        {
            case IOpenable.OpenState.CLOSED:
                _audioSource.PlayOneShot(_closeDoor);
                break;
            case IOpenable.OpenState.IS_OPENING:
                _audioSource.PlayOneShot(_openDoor);
                break;
            default:
                break;
        }
    }

    private void PlayLidSound(IOpenable.OpenState state)
    {
        switch (state)
        {
            case IOpenable.OpenState.IS_CLOSING:
                _audioSource.PlayOneShot(_gaslift);
                break;
            case IOpenable.OpenState.CLOSED:
                _audioSource.PlayOneShot(_slamLid);
                break;
            case IOpenable.OpenState.IS_OPENING:
                _audioSource.PlayOneShot(_openDoor);
                _audioSource.PlayOneShot(_gaslift);
                break;
            default:
                break;
        }
    }
}
