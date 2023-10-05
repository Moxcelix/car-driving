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
        private float _nativeGas;

        public Starter Starter => _starter;
        public float MaxRPM => _maxRPM;
        public float MaxTorque => _maxTorque;
        public bool Enabled => _starter.State == EngineState.STARTED;

        public float RPM { get; private set; }
        public float OutputRPM { get; private set; }
        public float Torque { get; private set; }

        public void Initialize()
        {
            _baseGas = 0.0f;
            _nativeGas = 0.0f;
        }

        public void Update(float inputGas,
            float outputRPM, float load, float deltaTime)
        {
            var localGas = GetLocalGas(inputGas) * _starter.RPMValue;
            var idlingGas = _idlingRPM / MaxRPM;
            var nativeRPM = SummGas(_nativeGas, idlingGas) * _cutoffRPM;
            var targetRPM = Mathf.Lerp(nativeRPM, outputRPM, load) * _starter.RPMValue;

            _starter.Update();

            UpdateNativeGas(localGas, deltaTime);
            UpdateBaseGas(deltaTime);
            UpdateTorque(localGas, outputRPM, load);
            UpdateRPM(targetRPM, deltaTime);
        }

        private void UpdateNativeGas(float localGas, float deltaTime)
        {
            _nativeGas = Mathf.Lerp(_nativeGas, localGas, deltaTime * _mobility);
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
            return additionalGas + (mainGas * (1.0f - additionalGas));
        }

        private float GetResistance(float outputRPM, float load)
        {
            return 1.0f - _resistanceCurve.Evaluate(outputRPM / MaxRPM) * load;
        }

        private void UpdateRPM(float targetRPM, float deltaTime)
        {
            RPM = Mathf.Lerp(RPM, targetRPM, deltaTime * _mobility);

            if (RPM > _cutoffRPM)
            {
                Torque = 0;
            }
        }

        private void UpdateTorque(float localGas, float outputRPM, float load)
        {
            var inputResistance = GetResistance(outputRPM, load);

            Torque = outputRPM > MaxRPM ? 0.0f :
                localGas * inputResistance * MaxTorque;
        }
    }
}
