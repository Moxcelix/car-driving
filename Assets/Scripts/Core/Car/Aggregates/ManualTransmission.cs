using UnityEngine;

namespace Core.Car
{
    public enum ManualTransmissionMode
    {
        GEAR,
        REVERSE,
        NEUTRAL,
    }

    public class ManualTransmission : Transmission
    {
        [SerializeField] private Gear[] _gears;
        [SerializeField] private float _reverseGearRatio = 3.44f;
        [SerializeField] private float _idlingRMP = 800;
        [SerializeField] private float _lastGearRatio = 3.574f;

        [SerializeField] private Pedal _clutchPedal;
        [SerializeField] private AnimationCurve _clutchDisplacement;
        [SerializeField] private AnimationCurve _clutchFeedback;
        [SerializeField][Range(0.0f, 1.0f)] private float _clutchWear;

        private float _inputTorque = 0;
        private float _inputRPM = 0;
        private float _outputRPM = 0;
        private int _currentGear = 0;

        private Vector2Int _selectorPosition = Vector2Int.zero;

        public ManualTransmissionMode Mode { get; private set; } = ManualTransmissionMode.NEUTRAL;

        public Pedal ClutchPedal => _clutchPedal;


        private void Update()
        {
            CurrentGear = _currentGear;

            IsReverse = Mode == ManualTransmissionMode.REVERSE;

            UpdateGear();
            UpdateTorque(_inputTorque, _outputRPM);
            UpdateBrake();
        }

        public override float GetRatio()
        {
            return Mode switch
            {
                ManualTransmissionMode.REVERSE =>
                    -_reverseGearRatio * _lastGearRatio,
                ManualTransmissionMode.GEAR =>
                    _gears[_currentGear].Ratio * _lastGearRatio,
                _ => 0,
            };
        }

        public override void SetValues(float inputTorque, float inputRPM, float outputRPM)
        {
            _inputTorque = inputTorque;
            _inputRPM = inputRPM;
            _outputRPM = outputRPM;
        }

        private void UpdateBrake()
        {
            var resistance = 0.01f;
            var clutch = GetClutchValue() > 0 ? 0.0f : 1.0f;
            var neutral = Mode == ManualTransmissionMode.NEUTRAL ? 1.0f : 0.0f;

            Brake = Mathf.Clamp01(clutch + neutral) * resistance;

            if (_outputRPM < 1)
            {
                Brake = 0;
            }
        }

        private void UpdateGear()
        {
            if (_selectorPosition.y == 0)
            {
                _currentGear = 0;
                Mode = ManualTransmissionMode.NEUTRAL;
            }
            else if (_selectorPosition.y == 1)
            {
                switch (_selectorPosition.x)
                {
                    case -2:
                        Mode = ManualTransmissionMode.REVERSE;
                        _currentGear = 0;
                        break;
                    case -1:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 0;
                        break;
                    case 0:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 2;
                        break;
                    case 1:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 4;
                        break;
                }
            }
            else
            {
                switch (_selectorPosition.x)
                {
                    case -1:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 1;
                        break;
                    case 0:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 3;
                        break;
                    case 1:
                        Mode = ManualTransmissionMode.GEAR;
                        _currentGear = 5;
                        break;
                }
            }
        }

        private float GetClutchValue()
        {
            return Mathf.Clamp01(1.0f - _clutchPedal.Value * (1.0f + _clutchWear));
        }

        private void UpdateTorque(float inputTorque, float outputRPM)
        {
            var nativeRPM = outputRPM * GetRatio();
            var coefficient = _clutchDisplacement.Evaluate(GetClutchValue());

            Load =
                Mode == ManualTransmissionMode.NEUTRAL ?
                0 : _clutchFeedback.Evaluate(GetClutchValue());
            Torque =
                coefficient *
                inputTorque *
                GetRatio();

            RPM = nativeRPM;
        }

        public override void SendLiteral(string literal)
        {
            switch(literal)
            {
                case "n":
                    _currentGear = 0;
                    Mode = ManualTransmissionMode.NEUTRAL;
                    break;
                case "r":
                    Mode = ManualTransmissionMode.REVERSE;
                    _currentGear = 0;
                    break;
                default:
                    if(int.TryParse(literal, out int gear))
                    {
                        if(gear >= 0 && gear < _gears.Length)
                        {
                            Mode = ManualTransmissionMode.GEAR;
                            _currentGear = gear;
                        }
                    }
                    break;
            }

            OnModeChange?.Invoke();
        }
    }
}