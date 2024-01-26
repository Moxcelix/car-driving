using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(Rigidbody))]
    public class RollBack : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private WheelCollider[] _wheelColliders;

        private readonly float _magnitude = 0.1f;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
           
        }

        private float GetTorque()
        {
            var torque = 0.0f;

            foreach(var wheel in _wheelColliders)
            {
                torque += wheel.motorTorque;
            }

            return torque / (float)_wheelColliders.Length; 
        }
    }
}