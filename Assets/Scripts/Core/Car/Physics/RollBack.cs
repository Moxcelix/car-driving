using UnityEngine;

namespace Core.Car
{
    public class RollBack : MonoBehaviour
    {

        [SerializeField] private WheelCollider[] _wheelColliders;

        private readonly float _magnitude = 0.1f;


        private void Update()
        {
            var torque = GetTorque();

            if (Mathf.Abs(torque) < _magnitude)
            {
                SetToqrque(torque * Mathf.Sin(transform.localEulerAngles.x));
            }
        }

        private void SetToqrque(float torque)
        {
            foreach(var wheel in _wheelColliders)
            {
                wheel.motorTorque = torque;
            }
        }

        private float GetTorque()
        {
            var torque = 0.0f;

            foreach (var wheel in _wheelColliders)
            {
                torque += wheel.motorTorque;
            }

            return torque / (float)_wheelColliders.Length;
        }
    }
}