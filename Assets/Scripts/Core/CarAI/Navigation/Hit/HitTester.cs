using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class HitTester
    {
        private readonly Transform _transform;
        private readonly Vector3 _hitRay;
        private readonly float _rayLength;

        public HitTester(Transform transform, Vector3 hitRay, float rayLength)
        {
            this._transform = transform;
            this._hitRay = hitRay;
            this._rayLength = rayLength;
        }

        public float GetHit<T>()
        {
            if (Physics.Raycast(_transform.position,
                _hitRay, out RaycastHit hit, _rayLength))
            {
                var tempMonoArray = hit.collider.
                    gameObject.GetComponents<MonoBehaviour>();

                foreach (var monoBehaviour in tempMonoArray)
                {
                    if (monoBehaviour is T)
                    {
                        return hit.distance;
                    }
                }
            }

            return float.PositiveInfinity;
        }
    }
}