using System;
using UnityEngine;

namespace Core.Car
{
    public enum TransmissionMode
    {
        NEUTRAL,
        DRIVING,
        REVERSE,
        PARKING
    }

    [System.Serializable]
    public class Transmission
    {
        private const float c_speedEps = 0.1f;

        [SerializeField] private TorqueConverter _torqueConverter;
        [SerializeField] private AnimationCurve _gasReactionCurve;
        [SerializeField] private Gear[] _gears;
        [SerializeField] private float _reverseGearRatio = 3.44f;
        [SerializeField] private float _idlingRMP = 800;
        [SerializeField] private float _lastGearRatio = 3.574f;

        private RatioShifter _ratioShifter;

        private float _accelerationFactor = 1;
        private float _speed = 0;
        private int _currentGear = 0;

        public Action<TransmissionMode> OnModeChange;

        public bool Lock { get; set; }
        public TransmissionMode Mode { get; private set; }
        public float Torque { get; private set; }
        public float RPM { get; private set; }
        public float Load { get; private set; }
        public float Brake { get; private set; }
        public int CurrentGear => _currentGear;

        public void Initialize()
        {
            _ratioShifter = new RatioShifter(_gears[0].Ratio);

            Mode = TransmissionMode.PARKING;
        }

        public float GetRatio()
        {
            return Mode switch
            {
                TransmissionMode.REVERSE =>
                    -_reverseGearRatio * _lastGearRatio,
                TransmissionMode.DRIVING =>
                    _ratioShifter.Value * _lastGearRatio,
                _ => 0,
            };
        }

        public void SwitchMode(TransmissionMode mode)
        {
            if (Lock)
            {
                return;
            }

            if (mode == Mode)
            {
                return;
            }

            if (Mathf.Abs(_speed) <= c_speedEps)
            {
                Mode = mode;

                OnModeChange?.Invoke(Mode);
            }
        }

        public void Update(float gasValue, float inputTorque, float inputRPM,
            float outputRPM, float speed, float deltaTime)
        {
            _speed = speed;
            _ratioShifter.Update();

            UpdateAccelerationFactor(gasValue);
            UpdateTorque(inputTorque, inputRPM, outputRPM, deltaTime);
            UpdateGearShifting(outputRPM);
            UpdateBrake();
        }

        private void UpdateAccelerationFactor(float acceleration)
        {
            _accelerationFactor = 1.0f +
                _gasReactionCurve.Evaluate(acceleration);
        }

        private void UpdateBrake()
        {
            Brake = Mode == TransmissionMode.PARKING ? 1.0f : 0.0f;
        }

        private void UpdateTorque(float inputTorque,
            float inputRPM, float outputRPM, float deltaTime)
        {
            var nativeRPM = outputRPM * GetRatio();

            _torqueConverter.Convert(nativeRPM, _idlingRMP, 6000);

            Load = 1.0f - _torqueConverter.FluidTransition;
            Torque =
                inputTorque *
                GetRatio() *
                _ratioShifter.Load *
               _torqueConverter.GetRatio();

            RPM = nativeRPM;
            //Mathf.Lerp(
                //nativeRPM,
                //inputRPM,
                //_torqueConverter.FluidTransition);
        }

        private void UpdateGearShifting(float rpm)
        {
            if (Mode != TransmissionMode.DRIVING)
            {
                _currentGear = 0;

                return;
            }

            rpm *= GetRatio();

            if (_ratioShifter.IsShifting)
            {
                return;
            }
            if (rpm > _gears[_currentGear].MaxRPM * _accelerationFactor)
            {
                UpshiftGear(1);
            }
            else if (rpm < _gears[_currentGear].MinRPM * _accelerationFactor)
            {
                DownshiftGear(1);
            }
        }

        private void UpshiftGear(int count)
        {
            if (count <= 0)
            {
                throw new System.ArgumentException();
            }

            if (_currentGear < _gears.Length - count)
            {
                if (_gears[_currentGear + 1].MinSpeed *
                    _accelerationFactor > _speed && RPM < 6000)
                {
                    return;
                }

                _currentGear += count;
                _ratioShifter.Shift(
                    _gears[_currentGear].Ratio,
                    _gears[_currentGear].ShiftSpeed);
            }
        }

        private void DownshiftGear(int count)
        {
            if (count <= 0)
            {
                throw new System.ArgumentException();
            }

            if (_currentGear > count - 1)
            {
                _currentGear -= count;
                _ratioShifter.Shift(
                    _gears[_currentGear].Ratio,
                    _gears[_currentGear].ShiftSpeed);
            }
        }
    }
}