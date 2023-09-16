using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class HitTester : MonoBehaviour, IHitTester
    {
        [SerializeField] private float _rayLength;

        private RaycastHit _hit;

        public bool IsHited { get; private set; }

        public float HitDistance { get; private set; }

        private void Update()
        {
            IsHited = Physics.Raycast(transform.position,
                transform.forward, out _hit, _rayLength);

            HitDistance = IsHited ? _hit.distance : float.PositiveInfinity;
        }

        public float GetHit<T>()
        {
            var tempMonoArray = _hit.collider.
                gameObject.GetComponents<MonoBehaviour>();

            foreach (var monoBehaviour in tempMonoArray)
            {
                if (monoBehaviour is T)
                {
                    return _hit.distance;
                }
            }

            return float.PositiveInfinity;
        }

        private void OnDrawGizmos()
        {
            const float arrowHeadLength = 0.5f;
            const float arrowHeadAngle = 30f;

            float length = IsHited ? _hit.distance : _rayLength;

            Gizmos.color = IsHited ? Color.green : Color.red;

            Gizmos.DrawLine(transform.position,
                transform.position + transform.forward.normalized * length);

            var right = Quaternion.LookRotation(transform.forward) *
                Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            var left = Quaternion.LookRotation(transform.forward) *
                Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(transform.position + transform.forward.normalized * length,
                right * arrowHeadLength);
            Gizmos.DrawRay(transform.position + transform.forward.normalized * length,
                left * arrowHeadLength);
        }
    }
}
