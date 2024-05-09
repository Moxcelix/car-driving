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
        [SerializeField] private float _clutchPowerCoefficient = 5.0f;

        [SerializeField] private Pedal _clutchPedal;
        [SerializeField] private AnimationCurve _clutchDisplacement;
        [SerializeField] private AnimationCurve _clutchFeedback;
        [SerializeField][Range(0.0f, 1.0f)] private float _clutchWear;

        private float _inputTorque = 0;
        private float _inputRPM = 0;
        private float _outputRPM = 0;
        private int _currentGear = 0;

        public ManualTransmissionMode Mode { get; private set; } = ManualTransmissionMode.NEUTRAL;

        public Pedal ClutchPedal => _clutchPedal;


        private void Update()
        {
            CurrentGear = _currentGear;

            IsReverse = Mode == ManualTransmissionMode.REVERSE;

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

        private float GetClutchValue()
        {
            return Mathf.Clamp01(1.0f - _clutchPedal.Value * (1.0f + _clutchWear));
        }

        private void UpdateTorque(float inputTorque, float outputRPM)
        {
            var clutchValue = GetClutchValue();
            var displacement = _clutchDisplacement.Evaluate(clutchValue);
            var feedback = _clutchFeedback.Evaluate(clutchValue);

            if(displacement < 0.0f)
            {
                displacement = 0.0f;
            }

            if(feedback < 0.0f)
            {
                feedback = 0.0f;
            }

            var nativeRPM = outputRPM * GetRatio();
            var transmitedRPM = displacement * _inputRPM;
            var powerCoefficient = transmitedRPM == 0 ? 0 :
                Mathf.Lerp(Mathf.Clamp01(
                    (transmitedRPM - nativeRPM) / transmitedRPM) * _clutchPowerCoefficient,
                    1, feedback);

            if(powerCoefficient < displacement)
            {
                powerCoefficient = displacement;
            }

            Debug.Log(powerCoefficient);
           
            Load =
                Mode == ManualTransmissionMode.NEUTRAL ?
                0 : feedback;

            Torque =
                powerCoefficient *
                inputTorque *
                GetRatio();

            RPM = nativeRPM;
        }

        public override bool SendLiteral(string literal)
        {
            switch(literal)
            {
                case "n":
                    _currentGear = 0;
                    Mode = ManualTransmissionMode.NEUTRAL;
                    break;
                case "r":
                    if (Mode != ManualTransmissionMode.NEUTRAL)
                    {
                        return false;
                    }
                    Mode = ManualTransmissionMode.REVERSE;
                    _currentGear = 0;
                    break;
                default:
                    if(Mode != ManualTransmissionMode.NEUTRAL)
                    {
                        return false;
                    }

                    if(int.TryParse(literal, out int gear))
                    {
                        if(gear > 0 && gear <= _gears.Length)
                        {
                            Mode = ManualTransmissionMode.GEAR;
                            _currentGear = gear - 1;
                        }
                    }
                    break;
            }

            OnModeChange?.Invoke();

            return true;
        }
    }
}