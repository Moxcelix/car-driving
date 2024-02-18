using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class TorqueConverter
    {
        [SerializeField] private float _maxRPM = 6000.0f;
        [SerializeField] private float _fluidDamp = 10.0f;
        [SerializeField] private float _maxRatio = 2.5f;

        private float _fluidTransition = 0;
        private float _targetCoefficient = 0;

        private bool _state = false;

        public float FluidTransition => _fluidTransition;

        public void Switch(bool state)
        {
            _state = state;
        }

        public void Update(float deltaTime)
        {
            _targetCoefficient =
                Mathf.Lerp(_targetCoefficient,
                _state ? 1.0f : 0.0f, deltaTime * _fluidDamp);
        }

        public void Convert(float inputRPM, float outputRPM)
        {
            if(inputRPM == 0 || outputRPM == 0)
            {
                _fluidTransition = 1.0f;

                return;
            }

            _fluidTransition = 1.0f - Mathf.Clamp01((outputRPM - inputRPM) / inputRPM);

            Debug.Log(_fluidTransition);

            _fluidTransition *= _targetCoefficient;
        }

        public float GetRatio()
        {
            return 1.0f +
                _fluidTransition *
                _fluidTransition *
                _maxRatio;
        }
    }
}
