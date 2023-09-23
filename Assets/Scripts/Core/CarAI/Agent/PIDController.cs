using System;
using UnityEngine;

namespace Core.CarAI.Agent
{
    public class PIDController
    {
        public enum DerivativeMeasurement
        {
            Velocity,
            ErrorRateOfChange
        }

        private readonly float _proportionalGain;
        private readonly float _integralGain;
        private readonly float _derivativeGain;

        private readonly float _outputMin = -1;
        private readonly float _outputMax = 1;
        private readonly float _integralSaturation;
        private readonly DerivativeMeasurement _derivativeMeasurement;

        private float _valueLast;
        private float _errorLast;
        private float _integrationStored;
        private float _velocity;
        private bool _derivativeInitialized;

        public void Reset()
        {
            _derivativeInitialized = false;
        }

        public float Update(float dt, float currentValue, float targetValue)
        {
            if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));

            float error = targetValue - currentValue;

            //calculate P term
            float P = _proportionalGain * error;

            //calculate I term
            _integrationStored = Mathf.Clamp(_integrationStored + (error * dt), -_integralSaturation, _integralSaturation);
            float I = _integralGain * _integrationStored;

            //calculate both D terms
            float errorRateOfChange = (error - _errorLast) / dt;
            _errorLast = error;

            float valueRateOfChange = (currentValue - _valueLast) / dt;
            _valueLast = currentValue;
            _velocity = valueRateOfChange;

            //choose D term to use
            float deriveMeasure = 0;

            if (_derivativeInitialized)
            {
                if (_derivativeMeasurement == DerivativeMeasurement.Velocity)
                {
                    deriveMeasure = -valueRateOfChange;
                }
                else
                {
                    deriveMeasure = errorRateOfChange;
                }
            }
            else
            {
                _derivativeInitialized = true;
            }

            float D = _derivativeGain * deriveMeasure;

            float result = P + I + D;

            return Mathf.Clamp(result, _outputMin, _outputMax);
        }

        float AngleDifference(float a, float b)
        {
            return (a - b + 540) % 360 - 180;   //calculate modular difference, and remap to [-180, 180]
        }

        public float UpdateAngle(float dt, float currentAngle, float targetAngle)
        {
            if (dt <= 0) throw new ArgumentOutOfRangeException(nameof(dt));
            float error = AngleDifference(targetAngle, currentAngle);

            // Ñalculate P term.
            float P = _proportionalGain * error;

            // Calculate I term.
            _integrationStored = Mathf.Clamp(_integrationStored + (error * dt), -_integralSaturation, _integralSaturation);
            float I = _integralGain * _integrationStored;

            // Calculate both D terms.
            float errorRateOfChange = AngleDifference(error, _errorLast) / dt;
            _errorLast = error;

            float valueRateOfChange = AngleDifference(currentAngle, _valueLast) / dt;
            _valueLast = currentAngle;
            _velocity = valueRateOfChange;

            // Choose D term to use.
            float deriveMeasure = 0;

            if (_derivativeInitialized)
            {
                deriveMeasure =
                    _derivativeMeasurement == DerivativeMeasurement.Velocity ?
                    -valueRateOfChange :
                    errorRateOfChange;
            }
            else
            {
                _derivativeInitialized = true;
            }

            float D = _derivativeGain * deriveMeasure;

            float result = P + I + D;

            return Mathf.Clamp(result, _outputMin, _outputMax);
        }
    }
}