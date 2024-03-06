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
    public class AutomaticTransmission : Transmission
    {
        private const float c_speedEps = 0.05f;

        [SerializeField] private Car _car;
        [SerializeField] private TorqueConverter _torqueConverter;
        [SerializeField] private AnimationCurve _gasReactionCurve;
        [SerializeField] private Gear[] _gears;
        [SerializeField] private float _reverseGearRatio = 3.44f;
        [SerializeField] private float _idlingRMP = 800;
        [SerializeField] private float _lastGearRatio = 3.574f;
        [SerializeField] private float _forcedSwitchRPM = 6000.0f;
        [SerializeField] private float _supportRPM = 1000.0f;

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

            _lock = !_car.Engine.Enabled;
            CurrentGear = _currentGear;
            IsReverse = Mode == AutomaticTransmissionMode.REVERSE;

            _speed = _car.GetSpeed() * 3.6f;
            _gasValue = _car.GasPedal.Value;

            _ratioShifter.Update();
            _torqueConverter.Switch((_currentGear == 0 || RPM < _supportRPM) && _car.Engine.Enabled);

            UpdateAccelerationFactor(_gasValue, Time.deltaTime);
            UpdateTorque(_inputTorque, _inputRPM, _outputRPM, Time.deltaTime);
            UpdateGearShifting(_outputRPM);
            UpdateBrake();
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
            if (Mode != AutomaticTransmissionMode.DRIVING &&
                Mode != AutomaticTransmissionMode.MANUAL &&
                Mode != AutomaticTransmissionMode.NEUTRAL)
            {
                _currentGear = 0;

                return;
            }

            if (_ratioShifter.IsShifting)
            {
                return;
            }

            var currentRPM = rpm * GetRatio();

            if (currentRPM <= _gears[_currentGear].MaxRPM * _accelerationFactor &&
                currentRPM >= _gears[_currentGear].MinRPM * _accelerationFactor &&
                _speed >= _gears[_currentGear].MinSpeed * _accelerationFactor)
            {
                return;
            }

            int targetGeer = GetGearByGasPressure(rpm, _car.GasPedal.Value, _car.BrakePedal.Value);
            if (targetGeer == _currentGear)
            {
                return;
            }

            if (Mode == AutomaticTransmissionMode.MANUAL)
            {
                if (_currentGear > 0 && RPM < _supportRPM)
                {
                    targetGeer = _currentGear - 1;
                }
                else
                {
                    return;
                }
            }

            if(targetGeer > _currentGear)
            {
                _currentGear++;
            }
            else
            {
                _currentGear--;
            }
            //_currentGear = targetGeer;
            _ratioShifter.Shift(
                _gears[_currentGear].Ratio,
                _gears[_currentGear].ShiftSpeed);
        }


        private int GetGearByGasPressure(float rpm, float gas, float brake)
        {
            float GetRpm(int i) => rpm * _gears[i].Ratio * _lastGearRatio;
            bool ValidateGear(int i, float validRpm) => GetRpm(i) >= validRpm;
            bool ValidateGearByMinimal(int i) => ValidateGear(i, _gears[i].MinRPM);
            bool ValidateGearByMaximal(int i, float max) => GetRpm(i) <= max;

            if (brake > 0.2f)
            {
                if (_currentGear == 0)
                {
                    return 0;
                }

                if (ValidateGearByMaximal(_currentGear, _supportRPM))
                {
                    return _currentGear - 1;
                }

                return _currentGear;
            }

            if (gas < 0.2f)
            {
                var targetGear = 0;

                for (int i = 0; i < _gears.Length && ValidateGearByMinimal(i); i++)
                {
                    targetGear = i;
                }

                return targetGear;
            }
            else if (gas < 0.7f)
            {
                var gasRepresentation = (gas - 0.2f) / (0.7f - 0.2f);
                var targetGear = 0;

                for (int i = 0; i < _gears.Length && ValidateGear(
                    i, 1000 + 3000 * gasRepresentation * gasRepresentation); i++)
                {
                    targetGear = i;
                }

                return targetGear;
            }
            else
            {
                var targetGear = 0;

                for (int i = 0; i < _gears.Length; i++)
                {
                    if (ValidateGearByMaximal(i, 5500))
                    {
                        targetGear = i;

                        break;
                    }
                }

                return targetGear;
            }
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
            var targetGeer = _currentGear;
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

        public override bool SendLiteral(string literal)
        {
            if (_lock || Mode == AutomaticTransmissionMode.PARKING && !_car.BrakePedal.IsPressed)
            {
                return false;
            }

            switch (literal)
            {
                case "p":
                    if (Mathf.Abs(_speed) > c_speedEps || !_car.BrakePedal.IsPressed)
                    {
                        return false;
                    }
                    Mode = AutomaticTransmissionMode.PARKING;
                    break;
                case "r":
                    if (_speed > c_speedEps)
                    {
                        return false;
                    }
                    Mode = AutomaticTransmissionMode.REVERSE;
                    break;
                case "n":
                    Mode = AutomaticTransmissionMode.NEUTRAL;
                    break;
                case "d":
                    if (_speed < -c_speedEps)
                    {
                        return false;
                    }
                    Mode = AutomaticTransmissionMode.DRIVING;
                    break;
                case "m":
                    if (Mode != AutomaticTransmissionMode.DRIVING)
                    {
                        return false;
                    }
                    Mode = AutomaticTransmissionMode.MANUAL;
                    break;
                case "+":
                    if (Mode != AutomaticTransmissionMode.MANUAL)
                    {
                        return false;
                    }
                    UpshiftGear(1);
                    break;
                case "-":
                    if (Mode != AutomaticTransmissionMode.MANUAL)
                    {
                        return false;
                    }
                    DownshiftGear(1);
                    break;
            }

            OnModeChange?.Invoke();

            return true;
        }
    }
}