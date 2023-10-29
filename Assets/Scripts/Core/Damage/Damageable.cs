using UnityEngine;

namespace Core.Damage
{
    public class Damageable : MonoBehaviour
    {
        private const float c_minVelocity = 0.1f;

        public delegate void DamageableDelegate();
        public event DamageableDelegate OnDamage;

        public bool IsDamaged { get; private set; }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("boom!");

            if (collision.relativeVelocity.magnitude > c_minVelocity)
            {
                IsDamaged = true;

                OnDamage?.Invoke();
            }
        }
    }
}