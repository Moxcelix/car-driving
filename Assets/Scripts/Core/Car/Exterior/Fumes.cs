using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Fumes : MonoBehaviour
    {
        [System.Serializable]
        private class ValueRange
        {
            [SerializeField] private float _min;
            [SerializeField] private float _max;
            [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0, 0, 1, 1);

            public float Evaluate(float t)
            {
                return Mathf.Lerp(_min, _max, _curve.Evaluate(t));
            }
        }

        [SerializeField] private Car _car;

        [SerializeField] private ValueRange _alphaRange;
        [SerializeField] private ValueRange _speedRange;
        [SerializeField] private ValueRange _scaleRange;
        [SerializeField] private ValueRange _rateRange;

        private ParticleSystem _particles;
        private ParticleSystemRenderer _renderer;

        private void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
            _renderer = _particles.GetComponent<ParticleSystemRenderer>();
        }

        private void Update()
        {
            var transition = _car.Engine.RPM / _car.Engine.MaxRPM;
            var main = _particles.main;
            var emission = _particles.emission;

            emission.enabled = _car.Engine.Starter.State == EngineState.STARTED;

            main.startSpeed = _speedRange.Evaluate(transition);
            main.startSize = _scaleRange.Evaluate(transition);
            emission.rateOverTime = _rateRange.Evaluate(transition);

            var color = _renderer.material.color;
            color.a = _alphaRange.Evaluate(transition);
            _renderer.material.color = color;
        }
    }
}