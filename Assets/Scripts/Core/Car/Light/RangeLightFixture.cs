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
            if (_state)
            {
                if (_transition < _range)
                {
                    _transition += _flashSpeed * deltaTime;
                }
                else if (_transition > _range + _flashSpeed * deltaTime * 2.0f)
                {
                    _transition -= _fadeSpeed * deltaTime;
                }
                else
                {
                    _transition = _range;
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

            Debug.Log(_transition);
        }
    }
}