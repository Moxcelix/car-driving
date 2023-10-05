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
            //var feedback = outputRPM * load;

            //Debug.Log(load);

            //_starter.Update();

            //UpdateTorque(inputGas, feedback);
            //UpdateRPM(outputRPM);

            _baseGas = Mathf.Lerp(_baseGas, _idlingRPM > RPM ? 0.1f : 0.0f, deltaTime);
            //{
            //    _baseGas = (_idlingRPM - RPM) / MaxRPM;

            //    if (_baseGas < 0)
            //    {
            //        _baseGas = 0;
            //    }
            //}

            //Debug.Log($"base gas = {_baseGas}");


            var engineInertia = 10;
            var localGas = _baseGas + (inputGas * (1.0f - _baseGas));
            _nativeGas = Mathf.Lerp(_nativeGas, localGas, deltaTime * engineInertia);
            var idlingGas = _idlingRPM / MaxRPM;
            var nativeRPM = (_nativeGas * (1.0f - idlingGas) + idlingGas) * _cutoffRPM;

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
                deltaTime * engineInertia);

            if (RPM > _cutoffRPM)
            {
                Torque = 0;
            }
        }

        private void UpdateTorque(float inputGas, float feedback)
        {
            var localGas = _baseGas + (inputGas * (1.0f - _baseGas));
            var inputResistance =
                1.0f - _resistanceCurve.Evaluate(feedback / MaxRPM);
            var inputRPM = localGas * MaxRPM * _starter.RPMValue;
            var torqueRPM = (inputRPM - feedback) * inputResistance;
            Torque = torqueRPM / MaxRPM * MaxTorque;
            OutputRPM = inputRPM;
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
