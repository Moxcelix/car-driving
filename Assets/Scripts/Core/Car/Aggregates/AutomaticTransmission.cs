using System;
using UnityEngine;

namespace Core.Car
{
    public enum AutomaticTransmissionMode
    {
        PARKING,
        REVERSE,
        NEUTRAL,
        DRIVING,
        MANUAL,
    }
    public class AutomaticTransmission : ITransmission
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

        private bool _lock = false;
        private float _accelerationFactor = 1;
        private float _speed = 0;
        private float _gasValue = 0;
        private float _inputTorque = 0;
        private float _inputRPM = 0;
        private float _outputRPM = 0;
        private int _currentGear = 0;

        public AutomaticTransmissionMode Mode { get; private set; }


        private void Start()
        {
            _ratioShifter = new RatioShifter(_gears[0].Ratio);

            Mode = AutomaticTransmissionMode.PARKING;
        }

        private void Update()
        {
            if (!_car.Engine.Enabled)
            {
                _currentGear = 0;
            }

            _lock = !_car.Engine.Enabled || !_car.BrakePedal.IsPressed;
            CurrentGear = _currentGear;
            IsReverse = Mode == AutomaticTransmissionMode.REVERSE;

            _speed = _car.GetSpeed() * 3.6f;
            _gasValue = _car.GasPedal.Value;

            _ratioShifter.Update();
            _torqueConverter.Switch(_currentGear == 0 && _car.Engine.Enabled);

            UpdateAccelerationFactor(_gasValue, Time.deltaTime);
            UpdateTorque(_inputTorque, _inputRPM, _outputRPM, Time.deltaTime);
            UpdateGearShifting(_outputRPM);
            UpdateBrake();
        }

        public override void SwitchUp()
        {
            if (Mode == AutomaticTransmissionMode.MANUAL)
            {
                UpshiftGear(1);

                OnModeChange?.Invoke();
            }
            else if (Mode == AutomaticTransmissionMode.DRIVING ||
                !_lock && Mathf.Abs(_speed) <= c_speedEps &&
                Mode != AutomaticTransmissionMode.PARKING)
            {
                Mode = (AutomaticTransmissionMode)((int)Mode - 1);

                OnModeChange?.Invoke();
            }
        }

        public override void SwitchDown()
        {
            if (Mode == AutomaticTransmissionMode.MANUAL)
            {
                DownshiftGear(1);

                OnModeChange?.Invoke();
            }
            else if (Mode == AutomaticTransmissionMode.NEUTRAL ||
                !_lock && Mathf.Abs(_speed) <= c_speedEps && 
                Mode != AutomaticTransmissionMode.DRIVING)
            {
                Mode = (AutomaticTransmissionMode)((int)Mode + 1);

                OnModeChange?.Invoke();
            }
        }

        public override void SwitchLeft()
        {
            if (Mode == AutomaticTransmissionMode.DRIVING)
            {
                Mode = AutomaticTransmissionMode.MANUAL;

                OnModeChange?.Invoke();
            }
        }

        public override void SwitchRight()
        {
            if (Mode == AutomaticTransmissionMode.MANUAL)
            {
                Mode = AutomaticTransmissionMode.DRIVING;

                OnModeChange?.Invoke();
            }
        }

        public override float GetRatio()
        {
            return Mode switch
            {
                AutomaticTransmissionMode.REVERSE =>
                    -_reverseGearRatio * _lastGearRatio,
                AutomaticTransmissionMode.DRIVING =>
                    _ratioShifter.Value * _lastGearRatio,
                AutomaticTransmissionMode.MANUAL =>
                _ratioShifter.Value * _lastGearRatio,
                _ => 0,
            };
        }

        public void LoadMode(AutomaticTransmissionMode mode)
        {
            Mode = mode;
        }

        public override void SetValues(float inputTorque, float inputRPM, float outputRPM)
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
            Brake = Mode == AutomaticTransmissionMode.PARKING ||
               (Mode != AutomaticTransmissionMode.NEUTRAL &&
               _car.Engine.Starter.State != EngineState.STARTED) ? 1.0f : 0.0f;
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
            if (Mode == AutomaticTransmissionMode.MANUAL)
            {
                return;
            }

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