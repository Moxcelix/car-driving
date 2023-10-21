using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class TorqueConverter
    {
        [SerializeField] private AnimationCurve _fluidCouplingCurve;
        [SerializeField] private float _fluidDamp = 10.0f;
        [SerializeField] private float _maxRatio = 2.5f;

        private float _fluidTransition = 0;

        public float FluidTransition => _fluidTransition;

        public void Convert(float currentRPM, float targetRPM, float maxRPM)
        {
            Debug.Log(_fluidTransition);

            if (currentRPM < targetRPM)
            {
                _fluidTransition = 1;
                return;
            }

            _fluidTransition = 1.0f - (Mathf.Clamp01(
                (currentRPM - targetRPM) / (maxRPM - targetRPM)));
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
