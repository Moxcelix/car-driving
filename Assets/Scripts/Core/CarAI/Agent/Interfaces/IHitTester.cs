namespace Core.CarAI
{
    public interface IHitTester
    {
        public float GetHit<T>();

        public bool IsHited { get; }

        public float HitDistance { get; }
    }
}