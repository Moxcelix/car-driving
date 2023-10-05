using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class Engine
    {
        [SerializeField] private AnimationCurve _resistanceCurve;
        [SerializeField] private AnimationCurve _differenceCurve;
        [SerializeField] private Starter _starter;
        [SerializeField] private float _maxRPM;
        [SerializeField] private float _cutoffRPM;
        [SerializeField] private float _idlingRPM;
        [SerializeField] private float _maxTorque;
        [SerializeField] private float _baseGasDose;
        [SerializeField] private float _mobility;

        private float _baseGas;
        private float _prevRPMDifference;
        private float _nativeGas = 0;

        public Starter Starter => _starter;
        public bool Enabled => _starter.State == EngineState.STARTED;
        public float MaxRPM => _maxRPM;
        public float MaxTorque => _maxTorque;

        public float RPM { get; private set; }
        public float OutputRPM { get; private set; }
        public float Torque { get; private set; }

        public void Initialize()
        {
            _prevRPMDifference = 0.0f;
        }

        public void Update(float inputGas, float outputRPM, float load, float deltaTime)
        {
            var localGas = GetLocalGas(inputGas);

            UpdateBaseGas(deltaTime);
            UpdateTorque(localGas, outputRPM, load, deltaTime);
        }

        private void UpdateBaseGas(float deltaTime)
        {
            var targetGas = _idlingRPM > RPM ?
                _baseGasDose : 0.0f;

            _baseGas = Mathf.Lerp(
                _baseGas,
                targetGas,
                deltaTime);
        }

        private float GetLocalGas(float inputGas)
        {
            return SummGas(inputGas, _baseGas);
        }

        private float SummGas(float mainGas, float additionalGas)
        {
            return additionalGas + (mainGas * (1.0f -  additionalGas));
        }

        private void UpdateTorque(float localGas, float outputRPM, float load, float deltaTime)
        {
            _nativeGas = Mathf.Lerp(_nativeGas, localGas, deltaTime * _mobility);

            var idlingGas = _idlingRPM / MaxRPM;
            var nativeRPM = SummGas(_nativeGas, idlingGas) * _cutoffRPM;

            var inputResistance =
               1.0f - _resistanceCurve.Evaluate(outputRPM / MaxRPM) * load;

            Torque = outputRPM > MaxRPM ?
                0 :
                localGas * MaxTorque * inputResistance;

            RPM = Mathf.Lerp(
                RPM,
                Mathf.Lerp(
                    nativeRPM,
                    outputRPM,
                    load),
                deltaTime * _mobility);

            if (RPM > _cutoffRPM)
            {
                Torque = 0;
            }
        }

        private void UpdateRPM(float value)
        {
            if (value > MaxRPM)
            {
                value = MaxRPM;
            }

            var inputDifference = value - RPM;
            var multiplier = _differenceCurve.Evaluate(
                Mathf.Abs((inputDifference - _prevRPMDifference) / MaxRPM));
            var difference = inputDifference * multiplier;

            RPM += difference;

            _prevRPMDifference = difference;
        }
    }
}
