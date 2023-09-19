using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    [RequireComponent(typeof(MeshRenderer))]
    public class RangeLightFixture : LightFixture
    { 
        private float _range = 0.5f;

        public void SetRange(float range)
        {
            this._range = Mathf.Clamp01(range);
        }

        protected override void UpdateTransition(float deltaTime)
        {
            var range = _state ? _range : 0.0f;
            var speed = _transition < range ? _flashSpeed : _fadeSpeed;

            _transition = Mathf.Lerp(_transition, range, deltaTime * speed);
        }
    }
}