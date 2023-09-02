using UnityEngine;
using Core.Animation;
using System;

namespace Core.Car
{
    public enum ParkingBrakeState
    {
        RAISED,
        LOWERED,
        SWITCHING_UP,
        SWITCHING_DOWN
    }

    public class ParkingBrake : MonoBehaviour
    {
        [SerializeField] private Vector3 _startAngle;
        [SerializeField] private Vector3 _endAngle;
        [SerializeField] private float _openSpeed;

        private Vector3_LinearAnimation _lowerAnimation;
        private Vector3_LinearAnimation _raiseAnimation;

        public float Brake { get; private set; }
        public Vector3 StartAngle => _startAngle;
        public Vector3 EndAngle => _endAngle;
        public ParkingBrakeState State { get; private set; }

        public Action<ParkingBrakeState> OnBrakeSwitch;

        private void Awake()
        {
            State = ParkingBrakeState.RAISED;

            _lowerAnimation = new(StartAngle, EndAngle, _openSpeed,
                angles => transform.localEulerAngles = angles,
                () => SetState(ParkingBrakeState.LOWERED));
            _raiseAnimation = new(EndAngle, StartAngle, _openSpeed,
                angles => transform.localEulerAngles = angles,
                () => SetState(ParkingBrakeState.RAISED));
        }

        private void Update()
        {
            Brake = State == ParkingBrakeState.LOWERED ? 0.0f : 1.0f;
        }

        public void Switch(bool isUp)
        {
            if (isUp)
            {
                if (State == ParkingBrakeState.LOWERED)
                {
                    Raise();
                }
            }
            else 
            {
                if (State == ParkingBrakeState.RAISED)
                {
                    Lower();
                }
            }
        }

        private void SetState(ParkingBrakeState state)
        {
            if(State == state)
            {
                return;
            }

            State = state;

            OnBrakeSwitch?.Invoke(State);
        }

        private void Lower()
        {
            if (State != ParkingBrakeState.RAISED)
            {
                return;
            }

            SetState(ParkingBrakeState.SWITCHING_DOWN);

            StartCoroutine(_lowerAnimation.GetAnimationCoroutine());
        }

        private void Raise()
        {
            if (State != ParkingBrakeState.LOWERED)
            {
                return;
            }

            SetState(ParkingBrakeState.SWITCHING_UP);

            StartCoroutine(_raiseAnimation.GetAnimationCoroutine());
        }
    }
}