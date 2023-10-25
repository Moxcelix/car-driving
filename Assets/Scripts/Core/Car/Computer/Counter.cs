namespace Core.Car
{
    public class Counter
    {
        private float _t;
        private float _maxCount;

        public delegate void CounterCallback();
        public event CounterCallback OnCounterEnd;

        public Counter(float maxCount)
        {
            _t = 0;
            _maxCount = maxCount;
        }

        public void Update(float deltaTime)
        {
            _t += deltaTime;

            if (_t > _maxCount)
            {
                _t = 0;
                OnCounterEnd?.Invoke();
            }
        }
    }
}