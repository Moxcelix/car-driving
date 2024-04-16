using UnityEngine;

namespace Core.Car
{
    [RequireComponent(typeof(WheelCollider))]
    public class Tire : MonoBehaviour
    {
        [SerializeField] private float _maxForwardStiffness = 1.0f;
        [SerializeField] private float _maxSidewaysStiffness = 1.0f;
        [SerializeField] private float _maxStiffnessRPM = 500.0f;

        private float _forwardStiffness;
        private float _sidewaysStiffness;

        private WheelCollider _wheelCollider;

        private void Awake()
        {
            _wheelCollider = GetComponent<WheelCollider>();

            _forwardStiffness = _wheelCollider.forwardFriction.stiffness;
            _sidewaysStiffness = _wheelCollider.sidewaysFriction.stiffness;
        }

        private void FixedUpdate()
        {
            var transition = Mathf.Clamp01(_wheelCollider.rpm / _maxStiffnessRPM);

            var forwardFriction = _wheelCollider.forwardFriction;
            var sidewaysFriction = _wheelCollider.sidewaysFriction;

            forwardFriction.stiffness = Mathf.Lerp(_forwardStiffness, _maxForwardStiffness, transition);
            sidewaysFriction.stiffness = Mathf.Lerp(_sidewaysStiffness, _maxSidewaysStiffness, transition);

            _wheelCollider.forwardFriction = forwardFriction;
            _wheelCollider.sidewaysFriction = sidewaysFriction;
        }
    }
}