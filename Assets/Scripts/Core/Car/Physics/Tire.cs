using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(WheelCollider))]
    public class Tire : MonoBehaviour
    {
        [SerializeField] private float _maxForwardStiffness = 1.0f;
        [SerializeField] private float _maxSideStiffness = 1.0f;

        private WheelCollider _wheelCollider;

        private void Awake()
        {
            _wheelCollider = GetComponent<WheelCollider>();
        }

        private void FixedUpdate()
        {
            
        }
    }
}