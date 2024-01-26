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
        [SerializeField] private GameObject _selectorPos_N;
        [SerializeField] private GameObject _selectorPos_NL;
        [SerializeField] private GameObject _selectorPos_NLL;
        [SerializeField] private GameObject _selectorPos_R;
        [SerializeField] private GameObject _selectorPos_NR;
        [SerializeField] private GameObject _selectorPos_1;
        [SerializeField] private GameObject _selectorPos_2;
        [SerializeField] private GameObject _selectorPos_3;
        [SerializeField] private GameObject _selectorPos_4;
        [SerializeField] private GameObject _selectorPos_5;
        [SerializeField] private GameObject _selectorPos_6;

        [SerializeField] private Gear[] _gears;
        [SerializeField] private float _reverseGearRatio = 3.44f;
        [SerializeField] private float _idlingRMP = 800;
        [SerializeField] private float _lastGearRatio = 3.574f;

        [SerializeField] private Pedal _clutchPedal;
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
            UpdateSelector();
            UpdateTorque(_inputTorque, _outputRPM);
            UpdateBrake();
        }

        public override void SwitchUp()
        {
            if (_selectorPosition.y < 1)
            {
                _selectorPosition.y += 1;

                OnModeChange?.Invoke();
            }
        }

        public override void SwitchDown()
        {
            if (_selectorPosition.y > -1 && _selectorPosition.x > -2)
            {
                _selectorPosition.y -= 1;

                OnModeChange?.Invoke();
            }
            else if (_selectorPosition.y > 0)
            {
                _selectorPosition.y -= 1;

                OnModeChange?.Invoke();
            }
        }

        public override void SwitchLeft()
        {
            if (_selectorPosition.y != 0)
            {
                return;
            }

            if (_selectorPosition.x > -2)
            {
                _selectorPosition.x -= 1;
            }
        }

        public override void SwitchRight()
        {
            if (_selectorPosition.y != 0)
            {
                return;
            }

            if (_selectorPosition.x < 1)
            {
                _selectorPosition.x += 1;
            }
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
        }

        private void UpdateSelector()
        {
            _selectorPos_N.SetActive(_selectorPosition == new Vector2Int(0, 0));
            _selectorPos_NL.SetActive(_selectorPosition == new Vector2Int(-1, 0));
            _selectorPos_NLL.SetActive(_selectorPosition == new Vector2Int(-2, 0));
            _selectorPos_R.SetActive(_selectorPosition == new Vector2Int(-2, 1));
            _selectorPos_NR.SetActive(_selectorPosition == new Vector2Int(1, 0));
            _selectorPos_1.SetActive(_selectorPosition == new Vector2Int(-1, 1));
            _selectorPos_2.SetActive(_selectorPosition == new Vector2Int(-1, -1));
            _selectorPos_3.SetActive(_selectorPosition == new Vector2Int(0, 1));
            _selectorPos_4.SetActive(_selectorPosition == new Vector2Int(0, -1));
            _selectorPos_5.SetActive(_selectorPosition == new Vector2Int(1, 1));
            _selectorPos_6.SetActive(_selectorPosition == new Vector2Int(1, -1));
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

        private float GetClutchPowerUpCoefficient()
        {
            var inputRPM = _inputRPM;
            var outputRPM = _outputRPM * GetRatio();

            return inputRPM == 0 ? 1.0f : (inputRPM - outputRPM) / inputRPM + 1.0f;
        }

        private void UpdateTorque(float inputTorque, float outputRPM)
        {
            var nativeRPM = outputRPM * GetRatio();
            var coefficient = GetClutchPowerUpCoefficient();    

            Load = Mode == ManualTransmissionMode.NEUTRAL ? 0 : GetClutchValue();
            Torque =
                inputTorque *
                GetRatio() *
                GetClutchValue();

            RPM = nativeRPM;
        }
    }
}