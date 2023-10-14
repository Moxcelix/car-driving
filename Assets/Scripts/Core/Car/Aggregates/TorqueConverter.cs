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

        public void Convert(float currentRPM, float targetRPM, float deltaTime)
        {
            Debug.Log(_fluidTransition);
            if (currentRPM < targetRPM)
            {
                _fluidTransition = 1;
                return;
            }

            if (currentRPM > targetRPM * 2)
            {
                _fluidTransition = 0;
                return;
            }

            _fluidTransition = Mathf.Lerp(1, 0, (currentRPM - targetRPM) / targetRPM);
            return;

            if (currentRPM < targetRPM - 50)
            {
                //_fluidTransition = _fluidCouplingCurve.Evaluate(
                //   (targetRPM - currentRPM) / targetRPM);
                _fluidTransition = Mathf.Lerp(
                    _fluidTransition, 1, deltaTime * _fluidDamp);
            }
            else if (currentRPM > targetRPM + 50)
            {
                _fluidTransition = Mathf.Lerp(
                   _fluidTransition, 0, deltaTime * _fluidDamp);
            }
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
