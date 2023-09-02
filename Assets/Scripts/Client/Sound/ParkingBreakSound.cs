
using Core.Car;
using System;
using UnityEngine;

[Serializable]
public class ParkingBrakeSound
{
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _parkingBrakeOn;
    [SerializeField] private AudioClip _parkingBrakeOff;

    private ParkingBrake _parkingBrake;

    public void Initialize(ParkingBrake parkingBrake)
    {
        _parkingBrake = parkingBrake;

        _parkingBrake.OnBrakeSwitch += PlayParkingBrakeSound;
    }

    public void Destroy()
    {
        _parkingBrake.OnBrakeSwitch -= PlayParkingBrakeSound;
    }

    private void PlayParkingBrakeSound(ParkingBrakeState state)
    {
        switch (state)
        {
            case ParkingBrakeState.SWITCHING_UP:
                _audioSource.PlayOneShot(_parkingBrakeOn);
                break;
            case ParkingBrakeState.SWITCHING_DOWN:
                _audioSource.PlayOneShot(_parkingBrakeOff);
                break;
            default:
                break;
        }
    }
}
