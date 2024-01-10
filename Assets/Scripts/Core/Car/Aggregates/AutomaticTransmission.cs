using System;
using UnityEngine;

namespace Core.Car
{
    public class AutomaticTransmission : MonoBehaviour, ITransmission
    {
        private const float c_speedEps = 0.1f;

        [SerializeField] private Car _car;
        [SerializeField] private TorqueConverter _torqueConverter;
        [SerializeField] private AnimationCurve _gasReactionCurve;
        [SerializeField] private Gear[] _gears;
        [SerializeField] private float _reverseGearRatio = 3.44f;
        [SerializeField] private float _idlingRMP = 800;
        [SerializeField] private float _lastGearRatio = 3.574f;
        [SerializeField] private float _forcedSwitchRPM = 6000.0f;

        private RatioShifter _ratioShifter;

        private float _accelerationFactor = 1;
        private float _speed = 0;
        private float _gasValue = 0;
        private float _inputTorque = 0;
        private float _inputRPM = 0;
        private float _outputRPM = 0;
        private int _currentGear = 0;

        public AutomaticTransmissionMode Mode { get; private set; }

        public float Torque { get; private set; }

        public float RPM { get; private set; }

        public float Load { get; private set; }

        public float Brake { get; private set; }

        public int CurrentGear => _currentGear;

        private void Update()
        {
            _speed = _car.GetSpeed() * 3.6f;
            _gasValue = _car.GasPedal.Value;

            _ratioShifter.Update();
            _torqueConverter.Switch(_currentGear <= 0);

            UpdateAccelerationFactor(_gasValue, Time.deltaTime);
            UpdateTorque(_inputTorque, _inputRPM, _outputRPM, Time.deltaTime);
            UpdateGearShifting(_outputRPM);
            UpdateBrake();
        }

        public void SwitchDown()
        {
            if (Mathf.Abs(_speed) <= c_speedEps && Mode != AutomaticTransmissionMode.DRIVING)
            {
                Mode = (AutomaticTransmissionMode)((int)Mode + 1);
            }
        }

        public void SwitchLeft()
        {

        }

        public void SwitchRight()
        {

        }

        public void SwitchUp()
        {
            if (Mathf.Abs(_speed) <= c_speedEps && Mode != AutomaticTransmissionMode.PARKING)
            {
                Mode = (AutomaticTransmissionMode)((int)Mode - 1);
            }
        }

        public float GetRatio()
        {
            return Mode switch
            {
                AutomaticTransmissionMode.REVERSE =>
                    -_reverseGearRatio * _lastGearRatio,
                AutomaticTransmissionMode.DRIVING =>
                    _ratioShifter.Value * _lastGearRatio,
                _ => 0,
            };
        }

        public void LoadMode(AutomaticTransmissionMode mode)
        {
            Mode = mode;
        }

        public void SetValues(float inputTorque, float inputRPM, float outputRPM)
        {
            _inputTorque = inputTorque;
            _inputRPM = inputRPM;
            _outputRPM = outputRPM;
        }

        private void UpdateAccelerationFactor(float acceleration, float deltaTime)
        {
            var factor = 1.0f + _gasReactionCurve.Evaluate(acceleration);

            _accelerationFactor =
                Mathf.Lerp(_accelerationFactor, factor, deltaTime);
        }

        private void UpdateBrake()
        {
            Brake = Mode == AutomaticTransmissionMode.PARKING ? 1.0f : 0.0f;
        }

        private void UpdateTorque(float inputTorque,
            float inputRPM, float outputRPM, float deltaTime)
        {
            var nativeRPM = outputRPM * GetRatio();

            _torqueConverter.Update(deltaTime);
            _torqueConverter.Convert(inputRPM, nativeRPM);

            Load = 1.0f - _torqueConverter.FluidTransition;
            Torque =
                inputTorque *
                GetRatio() *
                _ratioShifter.Load *
               _torqueConverter.GetRatio();

            RPM = nativeRPM;
        }

        private void UpdateGearShifting(float rpm)
        {
            if (Mode != AutomaticTransmissionMode.DRIVING)
            {
                _currentGear = 0;

                return;
            }

            var currentRPM = rpm * GetRatio();
            if (_ratioShifter.IsShifting)
            {
                return;
            }

            if (currentRPM <= _gears[_currentGear].MaxRPM * _accelerationFactor &&
                currentRPM >= _gears[_currentGear].MinRPM * _accelerationFactor &&
                _speed >= _gears[_currentGear].MinSpeed * _accelerationFactor)
            {
                return;
            }

            int targetGeer = GetGearByAcceleration(rpm);
            if (targetGeer == _currentGear)
            {
                return;
            }

            _currentGear = targetGeer;
            _ratioShifter.Shift(
                _gears[_currentGear].Ratio,
                _gears[_currentGear].ShiftSpeed);
        }

        private int GetGearByAcceleration(float rpm)
        {
            var targetGeer = 0;

            for (int i = 0; i < _gears.Length; i++)
            {
                var newRPM = rpm * _gears[i].Ratio * _lastGearRatio;

                if (newRPM < _gears[i].MinRPM * _accelerationFactor ||
                    _gears[i].MinSpeed * _accelerationFactor > _speed)
                {
                    break;
                }

                targetGeer = i;
            }

            return targetGeer;
        }

        private int GetGeerByOptimalValue(float rpm, float optimalValue)
        {
            var targetGeer = 0;
            var prevDifference = Mathf.Abs(optimalValue - rpm * GetRatio());

            for (int i = 0; i < _gears.Length; i++)
            {
                var newRPM = rpm * _gears[i].Ratio * _lastGearRatio;

                var difference = Mathf.Abs(optimalValue - newRPM);

                if (difference < prevDifference)
                {
                    prevDifference = difference;
                    targetGeer = i;
                }
            }

            return targetGeer;
        }

        private void UpshiftGear(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException();
            }

            if (_currentGear < _gears.Length - count)
            {
                if (_gears[_currentGear + 1].MinSpeed *
                    _accelerationFactor > _speed && RPM < _forcedSwitchRPM)
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