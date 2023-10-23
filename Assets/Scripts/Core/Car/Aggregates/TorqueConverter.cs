using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class TorqueConverter
    {
        [SerializeField] private AnimationCurve _fluidCouplingCurve;
        [SerializeField] private float _maxRPM = 6000.0f;
        [SerializeField] private float _fluidDamp = 10.0f;
        [SerializeField] private float _maxRatio = 2.5f;

        private float _fluidTransition = 0;

        public float FluidTransition => _fluidTransition;

        public void Convert(float inputRPM, float outputRPM)
        {
            Debug.Log(_fluidTransition);

            _fluidTransition = 1.0f - (Mathf.Clamp01(
                (inputRPM - outputRPM) / _maxRPM));
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
