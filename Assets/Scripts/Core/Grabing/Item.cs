using UnityEngine;

namespace Core.Grabing
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        private const int defaultLayer = 0;
        private const int ignoreRaycastLayer = 2;

        private Rigidbody _rigidbody;

        [SerializeField] private string _name;

        public string Hint => $"Взять {_name}";

        public bool IsTaken {  get; set; } = false;
        public bool IsLocked {  get; set; } = false;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rigidbody.isKinematic = IsLocked;
            _rigidbody.detectCollisions = !IsLocked;

            gameObject.layer = IsTaken? ignoreRaycastLayer : defaultLayer;
        }

        public void SetRotation(Vector3 angles)
        {
            transform.eulerAngles = angles;
        }
    }
}
