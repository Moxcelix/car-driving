using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    [RequireComponent(typeof(MeshRenderer))]
    public class LightFixture : MonoBehaviour
    {
        [SerializeField, ColorUsage(true, true)] protected Color _color;
        [SerializeField] protected float _minLight;
        [SerializeField] protected float _maxLight;
        [SerializeField] protected int _index;
        [SerializeField] protected float _flashSpeed = 5f;
        [SerializeField] protected float _fadeSpeed = 5f;

        protected MeshRenderer _renderer;
        protected bool _state = false;
        protected float _transition = 0f;

        public void SetLight(bool state)
        {
            this._state = state;
        }

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        private void FixedUpdate()
        {
            UpdateTransition(Time.fixedDeltaTime);
            UpdateLight(_minLight, _maxLight);
        }

        protected virtual void UpdateTransition(float deltaTime)
        {
            if (_state)
            {
                if (_transition < 1f)
                {
                    _transition += _flashSpeed * deltaTime;
                }
                else
                {
                    _transition = 1f;
                }
            }
            else
            {
                if (_transition > 0f)
                {
                    _transition -= _fadeSpeed * deltaTime;
                }
                else
                {
                    _transition = 0f;
                }
            }
        }

        protected void UpdateLight(float minLight, float maxLight)
        {
            var t = Mathf.Lerp(minLight, maxLight, _transition);
            var factor = Mathf.Pow(2, (t + 1));

            _renderer.materials[_index].SetColor("_EmissionColor", _color * factor);
        }
    }
}