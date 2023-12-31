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

        private void Update()
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

        public void TransmitTorque(float force)
        {
            _collider.motorTorque = force;
        }

        public void Brake(float force)
        {
            _collider.brakeTorque = force;
        }

        private float GetWheelSpeed()
        {
            return Mathf.Abs(2.0f * Mathf.PI *
                _collider.radius * _collider.rpm / 60.0f);
        }
    }
}
