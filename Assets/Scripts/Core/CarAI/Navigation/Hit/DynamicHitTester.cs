using UnityEngine;

namespace Core.CarAI.Navigation
{
    public class DynamicHitTester : MonoBehaviour, IHitTester
    {
        [SerializeField] private float _rayLength;

        private RaycastHit _hit;

        public float Coefficient { get; set; } = 1;

        public bool IsHited { get; private set; }

        public float HitDistance { get; private set; }

        public Vector3 Direction { get; private set; }

        private void Awake()
        {
            Direction = 
                transform.parent.worldToLocalMatrix.
                MultiplyVector(transform.forward);
        }

        private void Update()
        {
            IsHited = Physics.Raycast(transform.position,
                transform.forward, out _hit, _rayLength * Coefficient);

            HitDistance = IsHited ? _hit.distance : _rayLength;
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

            var coefficient = Coefficient < 0 ? 0 : Coefficient;

            float length = IsHited ? _hit.distance : _rayLength * coefficient;

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
