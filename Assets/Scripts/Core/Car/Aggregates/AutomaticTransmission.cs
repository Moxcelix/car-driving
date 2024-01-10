using UnityEngine;

namespace Core.Car
{
    public class AutomaticTransmission : MonoBehaviour, ITransmission
    {
        private const float c_speedEps = 0.1f;

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
        private int _currentGear = 0;

        public AutomaticTransmissionMode Mode { get; private set; }

        float ITransmission.Torque => throw new System.NotImplementedException();

        float ITransmission.RPM => throw new System.NotImplementedException();

        float ITransmission.Load => throw new System.NotImplementedException();

        float ITransmission.Brake => throw new System.NotImplementedException();

        int ITransmission.CurrentGear => throw new System.NotImplementedException();

        void ITransmission.SwitchDown()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchLeft()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchRight()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchUp()
        {
            throw new System.NotImplementedException();
        }

        float ITransmission.GetRatio()
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
    }
}