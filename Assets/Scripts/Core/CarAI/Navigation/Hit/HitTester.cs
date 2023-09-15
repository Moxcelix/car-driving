using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class HitTester
    {
        private readonly Transform _transform;
        private readonly Vector3 _hitRay;

        public HitTester(Transform transform, Vector3 hitRay)
        {
            this._transform = transform;
            this._hitRay = hitRay;
        }
    }
}