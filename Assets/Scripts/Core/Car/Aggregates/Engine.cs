using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class Engine
    {
        [SerializeField] private Starter _starter;
        [Header("Cutoff")]
        [SerializeField] private float _cutoffRPM;
        [SerializeField] private float _recoveryRPM;
        [Header("Characteristics")]
        [SerializeField] private float _maxRPM;
        [SerializeField] private float _idlingRPM;
        [SerializeField] private float _maxTorque;
        [SerializeField] private float _responsiveness;

        public Starter Starter => _starter;
        public float MaxRPM => _maxRPM;
        public float MaxTorque => _maxTorque;
        public bool Enabled => _starter.State == EngineState.STARTED;

        public float RPM { get; private set; }
        public float Torque { get; private set; }
        public float Consumption { get; private set; }

        private float _torqueRPM = 0.0f;
        private float _targetRPM = 0.0f;
        private bool _cutOff = false;

        public void Update(float inputGas,
            float outputRPM, float load, float deltaTime)
        {
            _starter.Update();

            inputGas = _cutOff ? 0.0f : inputGas;

            var idlingRPM = _idlingRPM * _starter.RPMValue;
            var virtualRPM = outputRPM > idlingRPM ? outputRPM : idlingRPM;
            var idleGas = _idlingRPM / MaxRPM * _starter.RPMValue;
            var targetRPM = SummGas(inputGas, idleGas) * MaxRPM * _starter.RPMValue;

            _torqueRPM = Mathf.Lerp(_torqueRPM, targetRPM, deltaTime);
            _targetRPM = Mathf.Lerp(_targetRPM, targetRPM, deltaTime);
            _targetRPM = Mathf.Lerp(_targetRPM, virtualRPM, load);

            var rpm = Mathf.Lerp(_targetRPM, outputRPM, load);
            var torque = (_torqueRPM - outputRPM) / MaxRPM * MaxTorque;

            RPM = Mathf.Lerp(RPM, rpm, deltaTime * _responsiveness);
            Torque = Mathf.Lerp(Torque, torque, deltaTime * _responsiveness);
            Consumption = Mathf.Lerp(Consumption, inputGas, deltaTime);

            UpdateCutoff();
        }

        private float SummGas(float mainGas, float additionalGas)
        {
            return additionalGas + (mainGas * (1.0f - additionalGas));
        }

        private void UpdateCutoff()
        {
            if (RPM > _cutoffRPM)
            {
                _cutOff = true;
            }

            if (RPM < _recoveryRPM)
            {
                _cutOff = false;
            }
        }

    }
}
