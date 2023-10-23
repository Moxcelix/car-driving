using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class Engine
    {
        [SerializeField] private Starter _starter;
        [SerializeField] private float _maxRPM;
        [SerializeField] private float _cutoffRPM;
        [SerializeField] private float _idlingRPM;
        [SerializeField] private float _maxTorque;

        public Starter Starter => _starter;
        public float MaxRPM => _maxRPM;
        public float MaxTorque => _maxTorque;
        public bool Enabled => _starter.State == EngineState.STARTED;

        public float RPM { get; private set; }
        public float OutputRPM { get; private set; }
        public float Torque { get; private set; }

        private float _torqueRPM = 0.0f;
        private float _targetRPM = 0.0f;

        public void Update(float inputGas,
            float outputRPM, float load, float deltaTime)
        {
            _starter.Update();

            var virtualOutputRPM = outputRPM > _idlingRPM ? outputRPM : _idlingRPM;
            var idleGas = _idlingRPM / MaxRPM * _starter.RPMValue;
            var targetRPM = SummGas(inputGas, idleGas) * MaxRPM;

            _torqueRPM = Mathf.Lerp(_torqueRPM, targetRPM, deltaTime);
            _targetRPM = Mathf.Lerp(_targetRPM, targetRPM, deltaTime);
            _targetRPM = Mathf.Lerp(_targetRPM, virtualOutputRPM, load);

            RPM = Mathf.Lerp(_targetRPM, outputRPM, load);
            Torque = (_torqueRPM - outputRPM) / MaxRPM * MaxTorque;
        }

        private float SummGas(float mainGas, float additionalGas)
        {
            return additionalGas + (mainGas * (1.0f - additionalGas));
        }

    }
}
