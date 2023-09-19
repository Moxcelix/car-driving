using UnityEngine;

namespace Core.CarAI
{
    public interface IHitTester
    {
        public float GetHit<T>();

        public bool IsHited { get; }

        public float HitDistance { get; }

        public Vector3 Direction { get; }
    }
}