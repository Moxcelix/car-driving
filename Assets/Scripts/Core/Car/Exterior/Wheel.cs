using UnityEngine;

namespace Core.Car
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private float _maxAngle = 30.0f;
        [SerializeField] private WheelCollider _collider;
        [SerializeField] private Transform _wheel;
        [SerializeField] private Transform _support;

        public float TurnAmount { get; set; } = 0;

        public float RPM { get; private set; } = 0;

        public void Handle()
        {
            var angle = TurnAmount * _maxAngle;

            _collider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            _wheel.SetPositionAndRotation(pos, rot);

            if (_support != null)
            {
                _support.localEulerAngles = new Vector3(0, angle, 0);
                _support.position = pos;
            }

            _collider.steerAngle = angle;
            _collider.steerAngle = angle;

            RPM = _collider.rpm;
        }

        public void LoadSyncState(Transform transform)
        {
            _wheel.SetLocalPositionAndRotation(
                transform.localPosition, transform.localRotation);
        }

        public Transform GetSyncState()
        {
            return _wheel;
        }

        public void TransmitTorque(float force)
        {
            _collider.motorTorque = force;
        }

        public void Brake(float force)
        {
            _collider.brakeTorque = force;
        }
    }
}
