using UnityEngine;

namespace Core.Grabing
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public bool IsTaken {  get; set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _rigidbody.isKinematic = IsTaken;
        }
    }
}
