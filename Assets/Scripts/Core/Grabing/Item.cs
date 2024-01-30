using UnityEngine;

namespace Core.Grabing
{
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        [SerializeField] private string _name;

        public string Hint => $"Взять {_name}";

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
